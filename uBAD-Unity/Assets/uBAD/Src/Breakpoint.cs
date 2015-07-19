using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	/// <summary>
	/// Creates a breakpoint which stops the reactor, used for debugging.
	/// Has no effect in builds.
	/// </summary>
	public class BreakPoint : Leaf
	{

		public override IEnumerator<NodeResult> NodeTask ()
		{
#if UNITY_EDITOR
			reactor.pause = true;
			Debug.Log("Paused at Breakpoint");
#endif
			yield return NodeResult.Success;
		}

		public override string ToString ()
		{
			return string.Format ("BREAK");
		}
	}

}