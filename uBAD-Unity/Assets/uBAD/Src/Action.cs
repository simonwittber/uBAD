using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Execute a IEnumerator<NodeResult> coroutine.
    /// </summary>
    public class Action : Leaf
    {

        ComponentMethodLookup method;

        public override void Apply (object[] args)
        {
            this.method = (ComponentMethodLookup)args[0];
        }

        public override IEnumerator<NodeResult> NodeTask ()
        {
            var task = (IEnumerator<NodeResult>)this.method.Invoke();
            while (task.MoveNext()) {
                if (task.Current == NodeResult.Continue) 
                    yield return NodeResult.Continue;
                else
                    yield return task.Current;
            }
            yield return NodeResult.Failure;
        }

        public override string ToString ()
        {
            return string.Format ("! {0}", method);
        }
    }
    
}