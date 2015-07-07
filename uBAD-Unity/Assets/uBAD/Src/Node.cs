using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    public class Node
    {
        public BADReactor reactor;
        public bool enabled = true;
        public NodeResult? state = null;
        public bool running = false;
        public Branch parent = null;
		public object[] arguments;

        public virtual void Apply(object[] arguments) {
			this.arguments = arguments;
        }

		public virtual T GetArg<T>(int index) {
			var obj = arguments[index];
			if(obj is Symbol) {
				var s = obj as Symbol;
				obj = reactor.blackboard.Get(s.name);
			}
			return (T)obj;
		}

		protected virtual void ResolveArguments() {
		}

        public Node() {
        }

        public IEnumerator<NodeResult> GetNodeTask ()
        {
			ResolveArguments();
#if UNITY_EDITOR
            if(reactor.debug)
                return DebugNodeTask ();
            else
                return NodeTask();
#else
            return NodeTask ();
#endif
        }
        
        IEnumerator<NodeResult> DebugNodeTask ()
        {
            var task = NodeTask ();
            while (true) {
                if (task.MoveNext ()) {
                    running = true;
                    state = task.Current;
                    if(state != NodeResult.Continue) {
                        yield return NodeResult.Continue;
                        running = false;
                        state = null;
                    }
                    yield return task.Current;

                } else {
                    throw new System.Exception(string.Format("Premature enumerator exit: {0}", task));
                }
            }
        }

        public virtual IEnumerator<NodeResult> NodeTask ()
        {
            yield break;
        }

        public virtual void Abort ()
        {
            state = null;
			running = false;
        }

        public override string ToString ()
        {
            return string.Format ("{0}", GetType ().Name);
        }


    }


}