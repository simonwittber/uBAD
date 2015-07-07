using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// The root node which must sit at the top of the tree.
    /// </summary>
    public class Root : Branch
    {
        public bool debug = false;

        public override IEnumerator<NodeResult> NodeTask ()
        {

            var children = this.children.ToArray ();

            while (true) {
                var childrenExist = false;
                for (var i=0; i<children.Length; i++) {
                    var child = children [i];
                    if(!child.enabled) continue;
                    childrenExist = true;
                    var task = child.GetNodeTask ();
                    while(task.MoveNext ()) {
                        if(task.Current == NodeResult.Continue)
                            yield return NodeResult.Continue;
                        else
                            break;
                    } 
                }
                if(!childrenExist)
                    yield return NodeResult.Continue;
            }
        }

    }
    
}