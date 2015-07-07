using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{

    /// <summary>
    /// Keep running the child until a failure result is returned.
    /// </summary>
    public class UntilFailure : Decorator
    {
        public override IEnumerator<NodeResult> NodeTask ()
        {
            if (ChildIsMissing ()) {
                yield return NodeResult.Failure;
                yield break;
            }
            while (true) {
                var task = children [0].GetNodeTask ();
                while (task.MoveNext ()) {
                    var t = task.Current;
                    if (t == NodeResult.Continue) {
                        yield return NodeResult.Continue;
                    } else if (t == NodeResult.Failure) {
                        yield return NodeResult.Failure;
                        yield break;
                    } else if (t == NodeResult.Success) {
                        yield return NodeResult.Continue;
                        break;
                    }
                }
                yield return NodeResult.Continue;
            }
        }
    }

}