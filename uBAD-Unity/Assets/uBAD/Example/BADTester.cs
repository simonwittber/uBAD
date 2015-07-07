using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BADTester : MonoBehaviour {

	public IEnumerator<BAD.NodeResult> DoSomeLongTask() {
		yield return Random.value>0.5f?BAD.NodeResult.Success:BAD.NodeResult.Failure;
	}

	public bool CheckSomeCondition() {
		return Random.value>0.05f;
	}
}
