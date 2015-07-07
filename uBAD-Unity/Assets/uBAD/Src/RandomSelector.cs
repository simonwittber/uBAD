using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// A selector which randomly shuffles it's children before executing.
    /// </summary>
    public class RandomSelector : Selector
    {
        public override IEnumerator<NodeResult> NodeTask ()
        {
            var rng = new System.Random ();  
            var n = children.Count;  
            while (n > 1) {  
                n--;  
                var k = rng.Next (n + 1);  
                var value = children [k];  
                children [k] = children [n];  
                children [n] = value;  
            }  

            var task = base.NodeTask ();
            while (task.MoveNext()) {
                yield return task.Current;
            }
        }

    }
}