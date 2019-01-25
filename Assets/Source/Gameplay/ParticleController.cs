using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
	void Update () {
		if (transform.position.y < -1) {
			ObjectPoolHelper.Destroy (gameObject);
		}
	}
}