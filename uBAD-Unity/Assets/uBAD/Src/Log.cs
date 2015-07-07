using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Outputs some text to the console, and pauses the editor if pause == true.
    /// </summary>
    public class Log : Leaf
    {
        string text = "";
        bool pause = false;


        public override void Apply (object[] args)
        {
            if(args.Length >= 1) 
                this.text = args[0].ToString();
            if(args.Length >= 2) 
                this.pause = (bool)args[1];
        }

		public override string ToString ()
		{
			return string.Format ("Log");
		}

        public override IEnumerator<NodeResult> NodeTask ()
        {
#if UNITY_EDITOR
            yield return NodeResult.Continue;
#endif
            Debug.Log (text);
            if(pause) {
                Debug.Break();
            }
            yield return NodeResult.Success;
        }
    }
}