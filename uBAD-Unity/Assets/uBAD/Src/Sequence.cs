using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Run all child nodes, returning a failure on the first child that fails.
    /// Returns success if all children succeed.
    /// </summary>
    public class Sequence : Branch
    {
        public override IEnumerator<NodeResult> NodeTask ()
        {
            for (int i = 0; i < children.Count; i++) {
                var child = children [i];
                if (!child.enabled)
                    continue;
                var task = child.GetNodeTask ();
                while (task.MoveNext ()) {
                    var t = task.Current;
                    if (t == NodeResult.Continue) {
                        yield return NodeResult.Continue;
                    } else
                        if (t == NodeResult.Failure) {
                        yield return NodeResult.Failure;
                        yield break;
                    } else if (t == NodeResult.Success) {
                        yield return NodeResult.Continue;
                        break;
                    }
                }
            }
            yield return NodeResult.Success;
        }
    }
    
}