using UnityEngine;
using System.Collections.Generic;

namespace BAD
{
    public class BB : Leaf
    {
		Symbol op, name;
        object value;

        public override void Apply (object[] args)
        {
            if(args.Length >= 1) 
                op = (Symbol)args[0];
            if(args.Length >= 2) 
				name = (Symbol)args[1];
			if(args.Length >= 3) 
				value = args[2];
        }

		public override string ToString ()
		{
			if(op == null || name == null || value == null) return "BB";
			return string.Format ("BB " + op.name + " " + name.name + " " + value.ToString());
		}

        public override IEnumerator<NodeResult> NodeTask ()
        {
#if UNITY_EDITOR
            yield return NodeResult.Continue;
#endif
			float v;
			if(value is float) 
				v = (float)value;
			else
				v = reactor.blackboard.Get((string)value);
            
			switch(op) {
			case "set":
				reactor.blackboard.Set(name, v);
				break;
			case "inc":
				reactor.blackboard.Inc(name, v);
				break;
			case "dec":
				reactor.blackboard.Dec(name, v);
				break;
			case "mul":
				reactor.blackboard.Mul(name, v);
				break;
			case "div":
				reactor.blackboard.Div(name, v);
				break;
			case "rnd":
				reactor.blackboard.Set(name, Random.value);
				break;
			}
            yield return NodeResult.Success;
        }
    }
}