using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController : MonoBehaviour {
	private GameObject fort;
	public float moveSpeed;
	public float fortImpactDamage;
	public GameObject pCannonball;
	public float distanceFromFortToStop;
	public float timeBetweenShots;

	private bool stopped = false;
	private float shotTimer = 0f;

	void OnEnable () {
		fort = GameObject.FindGameObjectWithTag ("Fort");
		stopped = false;
	}

	void Update () {
		if (!stopped) {
			Vector3 dir = Vector3.Normalize (fort.transform.position - transform.position);
			dir = new Vector3 (dir.x, 0, dir.z); // no vertical movement
			transform.Translate (dir * moveSpeed * Time.deltaTime, Space.World);
			if (GetDistanceFromFort () < distanceFromFortToStop) {
				stopped = true;
				shotTimer = timeBetweenShots;
			}
		} else {
			if (TickShotTimer ()) {
				Shoot ();
			}
		}

	}

	void Explode () {
		fort.GetComponent<FortController> ().DoDamage (fortImpactDamage);
		ObjectPoolHelper.Destroy (gameObject);
	}

	private Vector3 Get2DPosition (Vector3 v) {
		return new Vector3 (v.x, 0, v.z);
	}

	private float GetDistanceFromFort () {
		return Vector3.Distance (Get2DPosition (transform.position), Get2DPosition (fort.transform.position));
	}

	private void Shoot () {
		// shoot
		GameObject cannonball = ObjectPoolHelper.Instantiate (pCannonball, transform.position, Quaternion.identity);
		cannonball.GetComponent<CannonballController> ().Init (fort.transform.position, true, fortImpactDamage);
	}

	/**
		TickShotTimer()

		returns: true if a shot should be taken, false otherwise
	*/
	private bool TickShotTimer () {
		bool willShoot = false;
		shotTimer -= Time.deltaTime;
		if (shotTimer <= 0f) {
			shotTimer = timeBetweenShots;
			willShoot = true;
		}

		return willShoot;
	}
}