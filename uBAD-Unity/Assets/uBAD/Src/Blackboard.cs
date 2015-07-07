using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	public class Blackboard
	{

		
		Dictionary<string, float> blackboard = new Dictionary<string, float> ();

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
