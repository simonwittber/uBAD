using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Runs it's child node if a System.Func<bool> method returns true.
    /// </summary>
    public class If : Decorator
    {

        ComponentMethodLookup method;
        
        public override void Apply (object[] args)
        {
            this.method = (ComponentMethodLookup)args[0];
        }
            
        public override IEnumerator<NodeResult> NodeTask ()
        {
            if (ChildIsMissing ()) {
                yield return NodeResult.Failure;
            }
            var result = (bool)method.Invoke ();
            if (result) {
                var task = children [0].GetNodeTask ();
                while (task.MoveNext()) {
                    yield return task.Current;
                }
            } else {
                yield return NodeResult.Failure;
            }
        }

        public override string ToString ()
        {
            return string.Format ("IF ({0})", method);
        }

    }

}