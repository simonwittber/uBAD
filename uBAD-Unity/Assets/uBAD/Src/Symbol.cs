using UnityEngine;
using System.Collections.Generic;

namespace BAD
{
	public class Symbol
	{
		public readonly string name;

		public Symbol (string name)
		{
			this.name = name;
		}

		static Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol> ();
		
		public static Symbol Get (string name)
		{
			Symbol s;
			if (symbols.TryGetValue (name, out s)) {
				return s;
			} 
			s = symbols [name] = new Symbol (name);
			return s;
		}

		public static implicit operator string(Symbol s)
		{
			return s.name;
		}

		public override string ToString ()
		{
			return name;
		}
		
	}
}