using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace BAD
{
	public class Parser
	{
		static object EOL = new object ();
		static object OPEN = new object ();
		static object CLOSE = new object ();

		static public Branch Parse (GameObject go, string code)
		{
			var tokens = new Queue<string> (Tokenizer.Tokenize (code));
			List<object> arguments = new List<object> ();
			Branch branch = null;
			Node instance = null;
			while (tokens.Count > 0) {
				var token = tokens.Dequeue ();
				var atom = Atomize (token);
				if (atom == OPEN) {
					branch = instance as Branch;
					instance = null;
					DebugLog ("Open Branch: " + branch);
					continue;
				}
				if (atom == CLOSE) {
					DebugLog ("Close Branch: " + branch);
					if (branch.parent != null)
						branch = branch.parent;
					continue;
				}
				if (atom == EOL) {
					if (arguments.Count > 0 && instance != null) {
						DebugLog ("Applying arguments: " + string.Join (", ", (from i in arguments select i == null ? "NULL" : i.ToString ()).ToArray ()));
						instance.Apply (arguments.ToArray ());
						arguments.Clear ();
					}
					continue;
				}
				if (atom is Node) {
					DebugLog ("Node: " + atom + (atom is Branch ? "(Branch)" : "(Leaf)"));
					instance = atom as Node;
					instance.reactor = go.GetComponent<BADReactor>();
					if (branch != null) {
						DebugLog ("Adding to branch: " + branch);
						branch.Add (instance);
					}
					continue;
				}
				if (atom is ComponentMethodLookup) {
					var lookup = atom as ComponentMethodLookup;
					lookup.Resolve (go);
					arguments.Add (lookup);
					continue;
				}
				arguments.Add (atom);

			}
			return branch;
		}

		static object Atomize (string token)
		{
			if (token.StartsWith ("\"")) 
				return token;
			if (token == "true")
				return true;
			if (token == "false")
				return false;
			if (token == "\n")
				return EOL;
			if (token == "{")
				return OPEN;
			if (token == "}")
				return CLOSE;
			float value;
			if (float.TryParse (token, out value)) {
				return value;
			}
			if (token.Contains (".")) {
				return new ComponentMethodLookup (token.Split ('.'));
			}
			var typeName = GuessTypeName (token);
			try {
				var type = System.Type.GetType ("BAD." + typeName, true, true);
				return System.Activator.CreateInstance (type);
			} catch (System.TypeLoadException) {
				DebugLog ("Could not load type: " + typeName + ". Assuming symbol.");
			}
			return Symbol.Get (token);
		}

		static string GuessTypeName (string token)
		{
			switch (token) {
			case "?":
				return "Condition";
			case "!":
				return "Action";

			default:
				return token;
			}
		}

		static bool debug = true;

		static void DebugLog (object msg)
		{
			if (debug)
				Debug.Log (msg.ToString ());
		}

	}

}