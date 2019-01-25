using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutController : MonoBehaviour {
	private GameObject fort;
	public float moveSpeed;
	public float fortImpactDamage;

	void Start () {
		fort = GameObject.FindGameObjectWithTag ("Fort");
	}

	void Update () {
		Vector3 dir = Vector3.Normalize (fort.transform.position - transform.position);
		dir = new Vector3 (dir.x, 0, dir.z); // no vertical movement
		transform.Translate (dir * moveSpeed * Time.deltaTime, Space.World);

		if (Vector3.Distance (Get2DPosition (transform.position), Get2DPosition (fort.transform.position)) < 1.5f) {
			Explode ();
		}

	}

	void Explode () {
		fort.GetComponent<FortController> ().DoDamage (fortImpactDamage);
		ObjectPoolHelper.Destroy (gameObject);
	}

	private Vector3 Get2DPosition (Vector3 v) {
		return new Vector3 (v.x, 0, v.z);
	}
}