using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Runs child once then disables itself.
    /// </summary>
    public class Once : Decorator
    {

        
        public override IEnumerator<NodeResult> NodeTask ()
        {
            if (ChildIsMissing()) {
                yield return NodeResult.Failure;
                yield break;
            }
            
            var task = children [0].GetNodeTask ();
            while (task.MoveNext ()) {
				if(task.Current != NodeResult.Continue)
					enabled = false;
                yield return task.Current;
            }
            
        }

        public override string ToString ()
        {
            return string.Format ("Once");
        }
    }

}