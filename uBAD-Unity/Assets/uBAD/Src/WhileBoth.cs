using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{

    /// <summary>
    /// Executes its child while condition is true and the child has not failed.     
    /// </summary>
    public class WhileBoth : Decorator
    {

        ComponentMethodLookup method;
        
        public override void Apply (object[] args)
        {
            this.method = (ComponentMethodLookup)args[0];
        }

        public override IEnumerator<NodeResult> NodeTask ()
        {
            var child = children[0].GetNodeTask ();
            while (true) {
                if (!(bool)method.Invoke ()) {
					children[0].Abort();
                    yield return NodeResult.Failure;
                    yield break;
                } else {
                    if (child.MoveNext ()) {
                        yield return child.Current;
                    } else {
                        //This will happen if the child tasks ends without a fail or success.
                        yield return NodeResult.Failure;
                        yield break;
                    }
                }
            }
        }

        public override string ToString ()
        {
            return string.Format ("||? ({0})", method);
        }

    }

}