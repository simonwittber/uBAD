using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{

    /// <summary>
    /// Waits until a condition is true then executes its child.     
    /// </summary>
    public class WaitFor : Decorator
    {

        ComponentMethodLookup method;
        
        public override void Apply (object[] args)
        {
            this.method = (ComponentMethodLookup)args[0];
        }

        public override IEnumerator<NodeResult> NodeTask ()
        {
            while (true) {
                if ((bool)method.Invoke ()) 
                    break;
                else
                    yield return NodeResult.Continue;
            }

            var child = children[0].GetNodeTask ();
            while (true) {
                if (child.MoveNext ()) {
                    yield return child.Current;
                } else {
                    //This will happen if the child tasks ends without a fail or success.
                    yield return NodeResult.Failure;
                    yield break;
                }
            }
        }

        public override string ToString ()
        {
            return string.Format ("WaitFor {0}", method);
        }



    }
}