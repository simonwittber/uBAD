using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	public class Blackboard
	{

		Dictionary<Symbol, Label> labels = new System.Collections.Generic.Dictionary<Symbol, Label>();
		Dictionary<string, float> blackboard = new Dictionary<string, float> ();

		public Node GetLabel(Symbol name) {
			return labels[name].children[0];
		}

		public void CreateLabel(Label label) {
			labels[label.name] = label;
		}

		public Dictionary<string,float> Items {
			get {
				return blackboard;
			}
		}
		
		public void Set (string name, string value)
		{
			blackboard [name] = blackboard [value];
		}
		
		public void Set (string name, float value)
		{
			blackboard [name] = value;
		}

		public void Inc(string name, float amount) {
			blackboard[name] += amount;
		}

		public void Dec(string name, float amount) {
			blackboard[name] -= amount;
		}

		public void Mul(string name, float amount) {
			blackboard[name] *= amount;
		}

		public void Div(string name, float amount) {
			blackboard[name] /= amount;
		}

		public float Get(string name) {
			return blackboard[name];
		}
	}
}
