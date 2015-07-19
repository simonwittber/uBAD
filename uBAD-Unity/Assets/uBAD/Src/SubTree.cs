using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	/// <summary>
	/// Creates a subtree that will only execute when jumped to.
	/// </summary>
	public class SubTree : Label
	{
		public override IEnumerator<NodeResult> NodeTask ()
		{
			yield return NodeResult.Success;
		}

		public override string ToString ()
		{
			return string.Format ("SubTree:" + (name == null ? "NULL" : name));
		}
	}

}