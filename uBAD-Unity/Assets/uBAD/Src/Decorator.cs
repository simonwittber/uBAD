using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{

    public class Decorator : Branch
    {

        public override Branch Add (Node child)
        {
            base.Add (child);
            PruneChildren ();
            return this;
        }

        protected bool ChildIsMissing() {
            if(children.Count == 0) {
                Debug.LogWarning(string.Format("{0} has no child.", this));
                enabled = false;
                return true;
            }
            return false;
        }
        
        void PruneChildren ()
        {
            while (children.Count > 1) {
                children.RemoveAt (0);
            }
        }
    }

}