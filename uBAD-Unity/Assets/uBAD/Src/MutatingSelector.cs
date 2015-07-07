using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{

    /// <summary>
    /// A selector that re-orders it's children based on the mutation policy field.
    /// </summary>
    public class MutatingSelector : Branch
    {
    
        public MutationPolicy policy = MutationPolicy.MoveToTop;

        public override void Apply (object[] args)
        {
            this.policy = (MutationPolicy)args[0];
        }

        public override IEnumerator<NodeResult> NodeTask ()
        {
            foreach (var child in children.ToArray()) {
                var task = child.GetNodeTask ();
                while (task.MoveNext ()) {
                    var t = task.Current;
                    if (t == NodeResult.Continue) {
                        yield return NodeResult.Continue;
                    } else if (t == NodeResult.Failure) {
                        yield return NodeResult.Continue;
                        break;
                    } else if (t == NodeResult.Success) {
                        children.Remove (child);
                        if (policy == MutationPolicy.MoveToTop)
                            children.Insert (0, child);
                        else
                            children.Add (child);
                        yield return NodeResult.Success;
                        yield break;
                    }
                }
            }
            yield return NodeResult.Failure;
        }

        public override string ToString ()
        {
            return string.Format ("MutatingSelector ({0})", policy);
        }
    }

}