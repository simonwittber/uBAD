using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Run all nodes simultaneously, failing or succeeding based on the policy field.
    /// </summary>
    public class Parallel : Branch
    {

        public ParallelPolicy policy;
        
        public override void Apply (object[] args)
        {
            this.policy = (ParallelPolicy)args[0];
        }
        
        public override IEnumerator<NodeResult> NodeTask ()
        {
            var tasks = new List<IEnumerator<NodeResult>> ();
            var childMap = new Dictionary<IEnumerator<NodeResult>,Node> ();
            foreach (var c in children) {
                if (!c.enabled)
                    continue;
                var t = c.GetNodeTask ();
                childMap [t] = c; 
                tasks.Add (t);
            }
            
            while (true) {
                if (tasks.Count == 0) {
                    if (policy == ParallelPolicy.SucceedIfOneSucceeds) {
                        yield return NodeResult.Failure;
                    }
                    if (policy == ParallelPolicy.FailIfOneFails) {
                        yield return NodeResult.Success;
                    }
                }
                var toKill = new List<int> ();
                for (var i=0; i<tasks.Count; i++) {
                    var task = tasks [i];
                    if (task.MoveNext ()) {
                        if (policy == ParallelPolicy.SucceedIfOneSucceeds) {
                            if (task.Current != NodeResult.Continue) {
                                if (task.Current == NodeResult.Success) {
                                    foreach (var c in childMap.Keys) {
                                        if (c != task)
                                            childMap [c].Abort ();  
                                    }
                                    yield return NodeResult.Success;
                                } else if (task.Current == NodeResult.Failure) {
                                    toKill.Add (i);
                                }
                            }
                        }
                        if (policy == ParallelPolicy.FailIfOneFails) {
                            if (task.Current != NodeResult.Continue) {
                                if (task.Current == NodeResult.Failure) {
                                    foreach (var c in childMap.Keys) {
                                        if (c != task)
                                            childMap [c].Abort ();  
                                    }
                                    yield return NodeResult.Failure;
                                } else if (task.Current == NodeResult.Success)
                                    toKill.Add (i);
                            }
                        }
                        if (task.Current == NodeResult.Continue) {
                            
                        }
                    } else {
                        toKill.Add (i);
                    }
                }
                
                toKill.Reverse ();
                foreach (var i in toKill) {
                    childMap.Remove (tasks [i]);
                    tasks.RemoveAt (i);
                }
                toKill.Clear ();
                
                yield return NodeResult.Continue;
            }
        }

        public override string ToString ()
        {
            return string.Format ("|| ({0})", policy);
        }

    }

}