using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	/// <summary>
	/// Jumps to a label node.
	/// </summary>
	public class Jump : Leaf
	{
		public Symbol name;

		public override void Apply (object[] args)
		{
			if(args.Length >= 1) 
				name = (Symbol)args[0];
		}

		public override IEnumerator<NodeResult> NodeTask ()
		{
			var task = reactor.blackboard.GetLabel(name).GetNodeTask ();
			while (task.MoveNext ()) {
				yield return task.Current;
			}
            
		}

		public override string ToString ()
		{
			return string.Format ("Jump:" + (name==null?"NULL":name));
		}
	}

}