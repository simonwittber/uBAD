using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// BAD reactor searches all components on the gameobject which implement IReactorLoad, then executes the 
    /// graphs returned by those components.
    /// </summary>
    public class BADReactor : MonoBehaviour
    {
        public float tickDuration = 0.1f;
        [System.NonSerialized]
        public bool fastforward = false;
        [System.NonSerialized]
        public bool pause = false;
        [System.NonSerialized]
        public bool step = false;
        [System.NonSerialized]
        public bool debug = false;
        
        public float deltaTime { get; private set; }

        public List<Node> runningGraphs = new List<Node>();

		public TextAsset badCode;

		public Blackboard blackboard = new Blackboard();
        
		void Start ()
        {
			var root =  BAD.Parser.Parse(gameObject, badCode.text);
            Parse (root);
            runningGraphs.Add(root);
            StartCoroutine (RunReactor (root.GetNodeTask()));
        }

        void Parse(Node node) {
            node.reactor = this;
            var branch = node as Branch;
            if(branch != null) {
                foreach(var child in branch.children) {
                    Parse (child);
                }
            }
        }
        
        IEnumerator RunReactor (IEnumerator<BAD.NodeResult> task)
        {
            //This helps mitigate the stampeding herd problem when the level starts with many AI's waiting to start.
            yield return new WaitForSeconds (UnityEngine.Random.Range (0f, 0.15f));
            var delay = new WaitForSeconds (tickDuration);
            if (tickDuration <= 0)
                delay = null;
            while (true) {
                var startTick = Time.time;
                #if UNITY_EDITOR
                if(step) {
                    step = false;
                    pause = true;
                    yield return delay;
                    task.MoveNext ();
                }
                
                if(pause) {
                    yield return null;
                    continue;
                } 
                
                if(fastforward) {
                    fastforward = false;
                    yield return null;
                } else {
                    yield return delay;
                } 
                task.MoveNext ();
                
                #else
                if (fastforward) {
                    fastforward = false;
                    yield return null;
                } else {
                    yield return delay;
                } 
                task.MoveNext ();
                #endif
                deltaTime = Time.time - startTick;
            }
        }
        

    }
}


