using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Fails if child does not succeed within a time limit.
    /// </summary>
    public class TimeLimit : Decorator
    {

        public float seconds = 1;

        protected override void ResolveArguments ()
		{
			seconds = GetArg<float>(0);
		}
        
        public override IEnumerator<NodeResult> NodeTask ()
        {
            if (ChildIsMissing ()) {
                yield return NodeResult.Failure;
                yield break;
            }
            var start = Time.time;
            var task = children [0].GetNodeTask ();
            while (task.MoveNext ()) {
                var t = task.Current;
                if ((Time.time - start) > seconds) {
                    children [0].Abort ();
                    yield return NodeResult.Failure;
                    break;
                }
                if (t == NodeResult.Continue) {
                    yield return NodeResult.Continue;
                } else {
                    if (t == NodeResult.Failure) {
                        yield return NodeResult.Failure;
                        break;
                    }
                    if (t == NodeResult.Success) {
                        yield return NodeResult.Success;
                        break;
                    }
                }
            }
            
        }

        public override string ToString ()
        {
            return string.Format ("TimeLimit ({0})", seconds);
        }

    }

}