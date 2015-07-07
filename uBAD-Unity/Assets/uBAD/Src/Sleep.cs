using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	/// <summary>
	/// Sleep for a number of seconds before returning Success. Variance parameter modifies the duration by a random amount.
	/// </summary>
	public class Sleep : Leaf
	{

		public float seconds = 1;
		public float variance = 0.1f;

		protected override void ResolveArguments ()
		{
			seconds = GetArg<float> (0);
			variance = GetArg<float> (1);
		}

		public override IEnumerator<NodeResult> NodeTask ()
		{
			var N = Time.time;
			var T = seconds + UnityEngine.Random.Range (-variance / 2, variance / 2);
			while (Time.time - T <= N)
				yield return NodeResult.Continue;
			yield return NodeResult.Success;
		}

		public override string ToString ()
		{
			return string.Format ("Zzz ({0} {1})", seconds, variance);
		}
	}

}