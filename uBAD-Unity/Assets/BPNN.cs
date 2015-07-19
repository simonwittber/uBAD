using UnityEngine;
using System.Collections;
using System.Linq;


public class BPNN : MonoBehaviour
{
	public int inputCount = 2, hiddenCount = 3, outputCount = 1;
	
	void Awake ()
	{
		ConstructNN (inputCount, hiddenCount, outputCount);
	}

	int ni, nh, no;
	double[] ai, ah, ao;
	double[,] wi, wo, ci, co;

	double[,] NewMatrix (int I, int J, double fill=0.0)
	{
		var m = new double[I, J];
		for (var i=0; i<I; i++) {
			for (var j=0; j<J; j++) m [i, j] = fill;
		}
		return m;
	}

	void ConstructNN (int ni, int nh, int no)
	{
		ni += 1;
		this.ni = ni;
		this.nh = nh;
		this.no = no;
 
		ai = new double[ni];
		ah = new double[nh];
		ao = new double[no];
		for (var i=0; i<ni; i++) ai [i] = 1.0;
		for (var i=0; i<nh; i++) ah [i] = 1.0;
		for (var i=0; i<no; i++) ao [i] = 1.0;

		wi = NewMatrix (ni, nh);
		wo = NewMatrix (nh, no);
        
		for (var i=0; i<ni; i++) {
			for (var j=0; j<nh; j++) wi [i, j] = Random.Range (-0.2f, 0.2f);
		}

		for (var j=0; j<nh; j++) {
			for (var k=0; k<no; k++) wo [j, k] = Random.Range (-2.0f, 2.0f);
		}
 
		ci = NewMatrix (ni, nh);
		co = NewMatrix (nh, no);
	}
 
	double[] Calculate (double[] inputs)
	{
		if (inputs.Length != ni - 1) throw new System.Exception ("Wrong number of inputs.");
 
		for (var i=0; i<ni-1; i++) ai [i] = inputs [i];
 
		for (var j=0; j<nh; j++) {
			var sum = 0.0;
			for (var i=0; i<ni; i++) sum += ai[i] * wi [i, j];
			ah [j] = Sigmoid (sum);
		}

		for (var k=0; k<no; k++) {
			var sum = 0.0;
			for (var j=0; j<nh; j++) sum += ah [j] * wo [j, k];
			ao [k] = Sigmoid (sum);
		}
		return ao;
	}

	double[] NewArray (int length, double fill)
	{
		var a = new double[length];
		for (var i=0; i<length; i++) a [i] = fill;
		return a;
	}
 
	double BackPropagate (double[] targets, double N, double M)
	{
		if (targets.Length != no) throw new System.Exception ("Wrong number of target values.");
 
		var output_deltas = NewArray (no, 0.0);
        
		for (var k=0; k<no; k++) {
			var error = targets [k] - ao [k];
			output_deltas [k] = DSigmoid (ao [k]) * error;
		}
		var hidden_deltas = NewArray (nh, 0.0);
		for (var j=0; j<nh; j++) {
			var error = 0.0;
			for (var k=0; k<no; k++) error += output_deltas [k] * wo [j, k];
			hidden_deltas [j] = DSigmoid (ah [j]) * error;
		}
 
		for (var j=0; j<nh; j++) {
			for (var k=0; k<no; k++) {
				var change = output_deltas [k] * ah [j];
				wo [j, k] = wo [j, k] + N * change + M * co [j, k];
				co [j, k] = change;
			}
		}
		for (var i=0; i<ni; i++) {
			for (var j=0; j<nh; j++) {
				var change = hidden_deltas [j] * ai [i];
				wi [i, j] = wi [i, j] + N * change + M * this.ci [i, j];
				ci [i, j] = change;
			}
		}
		var e = 0.0;
		for (var k=0; k<targets.Length; k++) e += 0.5 * System.Math.Pow ((targets [k] - this.ao [k]), 2);
		return e;
	}

	void Train (TrainingPair[] patterns, int iter=100, double N=0.5, double M=0.1)
	{
		for (var i=0; i<iter; i++) {
			var error = 0.0;
			foreach (var p in patterns) {
				var inputs = p.inputs;
				var targets = p.result;
				Calculate (inputs);
				error = error + BackPropagate (targets, N, M);
			}
		}
	}

	void Test (TrainingPair[] patterns)
	{
		foreach (var p in patterns) {
			Debug.Log (p);
			var log = "Actual:" + string.Join(" ", (from i in Calculate (p.inputs) select i.ToString()).ToArray());
			Debug.Log (log);
		}
	}
    
	double Sigmoid (double x)
	{
		return System.Math.Tanh (x);
	}
 
	double DSigmoid (double y)
	{
		return 1.0 - System.Math.Pow (y, 2);
	}

	public class TrainingPair
	{
		public double[] inputs;
		public double[] result;

		public override string ToString ()
		{
			var repr = string.Join (" ", (from i in inputs select i.ToString ()).ToArray ());
			repr += " -> ";
			repr += string.Join (" ", (from i in result select i.ToString ()).ToArray ());
			return repr;
		}
	}
	
	void Start ()
	{
		var patterns = new TrainingPair[] {
			new TrainingPair () { inputs=new double[] {-1,-1}, result=new double[] { -1 }},
			new TrainingPair () { inputs=new double[] {-1,+1}, result=new double[] { +1 }},
			new TrainingPair () { inputs=new double[] {+1,-1}, result=new double[] { +1 }},
			new TrainingPair () { inputs=new double[] {+1,+1}, result=new double[] { -1 }}
		};
		Train (patterns, 10000);
		Test (patterns);
	}
}
