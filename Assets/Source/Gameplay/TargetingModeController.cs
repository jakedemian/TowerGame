using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingModeController : MonoBehaviour {
	public GameObject pGroundTarget;

	private int groundLayer = 1 << 9;
	private GameObject groundTarget = null;
	private int targetingMode = 0;

	private Vector3 groundTargetPosition;

	void Start () {

	}

	void Update () {
		UpdateGroundTargetMode ();
	}

	private void UpdateGroundTargetMode () {
		// get the position on the ground where the mouse is targeting
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, Mathf.Infinity, groundLayer)) {
			if (hit.point.z < 1.86f /* the fort's z pos, FIXME */ ) {
				SetTargetingMode (1);

				if (groundTarget == null) {
					groundTarget = Instantiate (pGroundTarget, new Vector3 (0, -10f, 0), Quaternion.identity);
				}

				groundTarget.transform.position = hit.point;
			} else {
				UnsetTargetingMode ();
			}

		}
	}

	public void SetTargetingMode (int mode) {
		targetingMode = mode;
	}

	public void UnsetTargetingMode () {
		if (targetingMode == 1 && groundTarget != null) {
			Destroy (groundTarget);
			groundTarget = null;
		}

		targetingMode = 0;
	}

	public Vector3 GetGroundTargetPosition () {
		if (groundTarget != null) {
			return groundTarget.transform.position;
		}
		return new Vector3 (0, -100, 0);
	}
}