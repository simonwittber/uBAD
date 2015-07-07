using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Invert the result of the child. Success becomes Failure and vice versa.
    /// </summary>
    public class Invert : Decorator {
        public override IEnumerator<NodeResult> NodeTask ()
        {
            if(ChildIsMissing()) {
                yield return NodeResult.Failure;
                yield break;
            }
            var task = children[0].GetNodeTask ();
            while (task.MoveNext ()) {
                var t = task.Current;
                if (t == NodeResult.Continue) {
                    yield return NodeResult.Continue;
                } else if (t == NodeResult.Failure) {
                    yield return NodeResult.Success;
                    yield break;
                } else if (t == NodeResult.Success) {
                    yield return NodeResult.Failure;
                    yield break;
                }
            }
        }

    }

}