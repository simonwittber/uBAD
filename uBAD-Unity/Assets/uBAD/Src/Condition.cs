using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Executes a System.Func<bool> method and returns the result as failure or success.
    /// </summary>
    public class Condition : Leaf
    {
        ComponentMethodLookup method;
        
        public override void Apply (object[] args)
        {
            this.method = (ComponentMethodLookup)args[0];
        }

        public override IEnumerator<NodeResult> NodeTask ()
        {
#if UNITY_EDITOR
            yield return NodeResult.Continue;
#endif
            yield return ((bool)method.Invoke()) ? NodeResult.Success : NodeResult.Failure;
        }

        public override string ToString ()
        {
            return string.Format ("? {0}", method);
        }
    }
    
}