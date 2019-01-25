using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : MonoBehaviour {
	public int health = 100;
	public GameObject pFloatingCombatText;

	private int currentHealth;
	private GameObject uiHealthBar;

	private void OnEnable () {
		currentHealth = health;

		CanvasLookAtCamera ();
		UpdateHealthBarUI ();
	}

	public void DealDamage (Damage damage) {
		float damageAfterResists = GetComponent<EnemyDamageResistances> ().GetDamageAfterResistances (damage);
		int damageInt = (int) damageAfterResists;

		GameObject fct = ObjectPoolHelper.Instantiate (
			pFloatingCombatText,
			transform.position + new Vector3 (0, 1f, 0),
			Quaternion.identity);
		fct.GetComponent<FloatingCombatTextController> ().SetText (damageInt.ToString ());

		currentHealth -= damageInt;

		if (currentHealth <= 0) {
			Kill ();
		}

		UpdateHealthBarUI ();
	}

	private void UpdateHealthBarUI () {
		float healthPercentage = (float) currentHealth / (float) health;
		float healthbarHeight = GetUIHealthBar ().GetComponent<RectTransform> ().rect.height;
		GetUIHealthBar ().GetComponent<RectTransform> ().sizeDelta = new Vector2 (GetHealthBarWidth () * healthPercentage, healthbarHeight);
	}

	private GameObject GetUIHealthBar () {
		if (uiHealthBar == null) {
			uiHealthBar = transform.GetChild (0).GetChild (0).GetChild (0).gameObject;
		}

		return uiHealthBar;
	}

	private float GetHealthBarWidth () {
		return transform.GetChild (0).GetChild (0).GetComponent<RectTransform> ().rect.width;
	}

	private void Kill () {
		// TODO lookup how much xp you get for this type of enemy
		PlayerExperienceHelper.AddExperience (10);

		gameObject.SetActive (false);
	}

	private void CanvasLookAtCamera () {
		Transform canvasTransform = transform.GetChild (0).transform;
		canvasTransform.LookAt (canvasTransform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.up);
		canvasTransform.Rotate (0, 180, 0);
	}
}