using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Runs child every second time.
    /// </summary>
    public class FlipFlop : Decorator
    {

		public bool run = true;

        public override IEnumerator<NodeResult> NodeTask ()
        {
            if (!run || ChildIsMissing()) {
                yield return NodeResult.Failure;
                yield break;
            }
			run = !run;
            var task = children [0].GetNodeTask ();
            while (task.MoveNext ()) {
                yield return task.Current;
            }
        }

        public override string ToString ()
        {
            return string.Format ("FlipFlip");
        }
    }

}