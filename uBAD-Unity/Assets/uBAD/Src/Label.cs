using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
	/// <summary>
	/// Creates a label which can be the target of a jump node.
	/// </summary>
	public class Label : Decorator
	{
		public Symbol name;

		public override void Apply (object[] args)
		{
			if(args.Length >= 1) 
				name = (Symbol)args[0];
			reactor.blackboard.CreateLabel(this);
		}

		public override IEnumerator<NodeResult> NodeTask ()
		{
			if (ChildIsMissing ()) {
				yield return NodeResult.Failure;
				yield break;
			}
            
			var task = children [0].GetNodeTask ();
			while (task.MoveNext ()) {
				yield return task.Current;
			}
            
		}

		public override string ToString ()
		{
			return string.Format ("Label:" + (name==null?"NULL":name));
		}
	}

}