using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelController : MonoBehaviour {
	public TextMeshProUGUI uiLevelDisplay;
	public TextMeshProUGUI uiXpRemainingDisplay;
	public GameObject uiXpBar;

	private int playerLevel;
	private int currentXp;
	void Start () {
		FetchAllLevelData ();
		UpdateUI ();
	}

	public void AddExperience (int xp) {
		bool isLevelUp = PlayerLevelData.AddExperience (xp);

		if (isLevelUp) {
			// TODO do level up stuff
		}
		FetchAllLevelData ();
		UpdateUI ();
	}

	private void FetchAllLevelData () {
		playerLevel = PlayerLevelData.GetPlayerLevel ();
		currentXp = PlayerLevelData.GetTotalExperience ();
	}

	private void UpdateUI () {
		// update level display
		uiLevelDisplay.text = "Level  " + playerLevel.ToString ();

		int thisLevelXp = PlayerLevelData.GetLevelXpThreshold (playerLevel);
		int nextLevelXp = PlayerLevelData.GetNextLevelThreshold ();
		uiXpRemainingDisplay.text = (currentXp - thisLevelXp).ToString () + "/" + (nextLevelXp - thisLevelXp).ToString ();

		float percentToNextLevel = (float) (currentXp - thisLevelXp) / (float) (nextLevelXp - thisLevelXp);
		Vector2 xpBarDimensions = uiXpBar.GetComponent<RectTransform> ().rect.size;
		uiXpBar.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta = new Vector2 (xpBarDimensions.x * percentToNextLevel, xpBarDimensions.y);
	}
}

public static class PlayerExperienceHelper {
	private static PlayerLevelController GetController () {
		return GameObject.FindGameObjectWithTag ("GameController").GetComponent<PlayerLevelController> ();
	}

	public static void AddExperience (int xp) {
		GetController ().AddExperience (xp);
	}
}