using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Runs each child in turn, returing a success on the first child that succeeds. 
    /// Returns failure if no child succeeds.
    /// </summary>
    public class Selector : Branch
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
                        yield return NodeResult.Continue;
                        break;
                    } else
                            if (t == NodeResult.Success) {
                        yield return NodeResult.Success;
                        yield break;
                    }
                }
            }
            yield return NodeResult.Failure;
        }
    }
}