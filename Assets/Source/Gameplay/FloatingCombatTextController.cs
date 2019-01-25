using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingCombatTextController : MonoBehaviour {

	private float timer;
	private float displayTime = 1f; // should be loaded from options or something

	private TextMeshProUGUI textMeshObj;

	public float liftSpeed;
	public float fontSize;

	void Start () {
		GetText ().fontSize = fontSize;
		LookAtCamera ();
	}

	void Update () {
		transform.Translate (Vector3.up * liftSpeed * Time.deltaTime);

		if (timer > 0f) {
			timer -= Time.deltaTime;
		}

		if (timer <= 0f) {
			ObjectPoolHelper.Destroy (gameObject);
		}
	}

	private void LookAtCamera () {
		Transform canvasTransform = transform.GetChild (0);
		canvasTransform.LookAt (canvasTransform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
		canvasTransform.Rotate (0, 180, 0);
	}

	public void SetText (string s) {
		timer = displayTime;
		GetText ().text = s;
	}

	private TextMeshProUGUI GetText () {
		if (textMeshObj == null) {
			textMeshObj = GetComponentInChildren<TextMeshProUGUI> ();
		}

		return textMeshObj;
	}
}