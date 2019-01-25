using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballController : MonoBehaviour {

	[HideInInspector]
	public Vector3 endPosition;
	private Vector3 startPosition;

	public float flightTime;
	public float maxHeight;
	private bool isEnemy = false;
	protected float Animation;
	private float fortDamage = 0f;

	// TODO it can look up its own speed, height, from current skill numeric modifiers 
	public void Init (Vector3 end, bool enemy = false, float fortDamage = 0f) {
		endPosition = end;
		isEnemy = enemy;
		this.fortDamage = fortDamage;
	}

	private void OnEnable () {
		startPosition = transform.position;
	}

	void Update () {
		Animation += Time.deltaTime;
		if (Animation > flightTime) {
			ReportImpact ();
			ObjectPoolHelper.Destroy (gameObject);
		}

		Animation = Animation % flightTime;
		transform.position = MathParabola.Parabola (
			startPosition,
			endPosition,
			maxHeight,
			Animation / flightTime);
	}

	private void ReportImpact () {
		if (!isEnemy) {
			GameObject.FindGameObjectWithTag ("GameController")
				.GetComponent<SkillActionController> ()
				.CallbackCannonballHit (endPosition);
		} else {
			// report the impact to the game controller or the fort?
			GameObject.FindGameObjectWithTag ("Fort").GetComponent<FortController> ().DoDamage (fortDamage);
			ObjectPoolHelper.Destroy (gameObject);
		}
	}
}