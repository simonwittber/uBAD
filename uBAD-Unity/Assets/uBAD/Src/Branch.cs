using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{

    public class Branch : Node
    {
        public List<Node> children = new List<Node> ();

        public virtual Branch Add (Node node)
        {
            node.parent = this;
            this.children.Add (node);
            return this;
        }

        public override void Abort ()
        {
            state = null;
			running = false;
            foreach (var c in children) {
                c.Abort (); 
            }
        }

    }
    
}