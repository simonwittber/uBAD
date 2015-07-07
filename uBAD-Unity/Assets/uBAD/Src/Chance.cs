using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	/// <summary>
	/// Only runs child if probability value is met.
	/// </summary>
	public class Chance : Decorator
	{

		public float probability = 0.5f;

		protected override void ResolveArguments ()
		{
			probability = GetArg<float> (0);
		}
        
		public override IEnumerator<NodeResult> NodeTask ()
		{
			if (ChildIsMissing ()) {
				yield return NodeResult.Failure;
				yield break;
			}
			if (Random.value <= probability) {
				var task = children [0].GetNodeTask ();
				while (task.MoveNext ()) {
					yield return task.Current;
				}
			} else {
				yield return NodeResult.Failure;
			}            
		}

		public override string ToString ()
		{
			return string.Format ("Chance ({0}%)", probability * 100f);
		}
	}

}