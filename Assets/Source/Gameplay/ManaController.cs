using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour {
	public GameObject uiManaBarPanelBackground;
	private GameObject uiManaBarPanelForeground;
	private Vector2 manaPanelMaxSize;

	private float manaRegenPerSecond;
	private float currentMana;
	private float maxMana;

	void Start () {
		uiManaBarPanelForeground = uiManaBarPanelBackground.transform.GetChild (0).gameObject;
		manaPanelMaxSize = uiManaBarPanelBackground.GetComponent<RectTransform> ().rect.size;

		// TODO find a better way to set these, probably on level init or something?
		manaRegenPerSecond = 5f;
		maxMana = 100f;

		// start with max mana
		currentMana = maxMana;

		SpendMana (30f);
	}

	void Update () {
		float lastManaVal = currentMana;
		if (currentMana < maxMana) {
			currentMana += manaRegenPerSecond * Time.deltaTime;
			if (currentMana > maxMana) {
				currentMana = maxMana;
			}
		}

		// only update UI if things actually changed
		if (lastManaVal != currentMana) {
			UpdateManaBarUI ();
		}
	}

	public float GetCurrentMana () {
		return currentMana;
	}

	public void SpendMana (float mana) {
		currentMana -= mana;
		UpdateManaBarUI ();
	}

	private void UpdateManaBarUI () {
		float newManabarWidth = (currentMana / maxMana) * manaPanelMaxSize.x;
		uiManaBarPanelForeground.GetComponent<RectTransform> ().sizeDelta = new Vector2 (newManabarWidth, manaPanelMaxSize.y);
	}
}

public static class ManaHelper {
	public static ManaController GetController () {
		return GameObject.FindGameObjectWithTag ("GameController").GetComponent<ManaController> ();
	}

	public static bool HasEnoughMana (float manaRequired) {
		bool hasEnoughMana = false;

		if (manaRequired < GetController ().GetCurrentMana ()) {
			hasEnoughMana = true;
		}

		return hasEnoughMana;
	}

	public static void SpendMana (float manaToSpend) {
		GetController ().SpendMana (manaToSpend);
	}
}