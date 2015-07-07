using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	/// <summary>
	/// Runs a child node if it's cooldown timer less than 0;
	/// When the child executes the timer is set to T+seconds. Used for rate limiting.
	/// </summary>
	public class Cooldown : Decorator
	{
		public float seconds = 1;
		float lastRunTime = -1;

		protected override void ResolveArguments ()
		{
			seconds = GetArg<float> (0);
		}
            
		public override IEnumerator<NodeResult> NodeTask ()
		{
			if (ChildIsMissing ()) {
				yield return NodeResult.Failure;
				yield break;
			}
			if (lastRunTime < 0)
				lastRunTime = Time.time;
			var T = Time.time - lastRunTime;
			if (T > seconds) {
				lastRunTime = Time.time;
				var task = children [0].GetNodeTask ();
				while (task.MoveNext ()) {
					var t = task.Current;
					if (t == NodeResult.Continue) {
						yield return NodeResult.Continue;
					} else if (t == NodeResult.Success) {
						yield return NodeResult.Success;
						break;
					} else if (t == NodeResult.Failure) {
						yield return NodeResult.Failure;
						break;
					}
				}
			} else {
				yield return NodeResult.Failure;
			}
		}

		public override string ToString ()
		{
			return string.Format ("Cooldown ({0})", seconds);
		}
	}



}