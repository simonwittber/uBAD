using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Run a child a number of times, regardless of success or failure. Always succeeds.
    /// </summary>
    public class Loop : Decorator
    {
        public int loops = 1;

        protected override void ResolveArguments ()
		{
			loops = (int)GetArg<float>(0);
		}

        public override IEnumerator<NodeResult> NodeTask ()
        {
            if (ChildIsMissing ()) {
                yield return NodeResult.Failure;
                yield break;
            }
            for (int i = 0; i < loops; i++) {
                var task = children [0].GetNodeTask ();
                while (task.MoveNext ()) {
                    var t = task.Current;
                    if (t == NodeResult.Continue) {
                        yield return NodeResult.Continue;
                    } else if (t == NodeResult.Failure) {
                        yield return NodeResult.Continue;
                        break;
                    } else if (t == NodeResult.Success) {
                        yield return NodeResult.Continue;
                        break;
                    }
                }
            }
            yield return NodeResult.Success;
        }

        public override string ToString ()
        {
            return string.Format ("Loop ({0})", loops);
        }

    }

}