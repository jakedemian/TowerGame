using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerLevelData {
	private static int playerLevel = 1;
	private static int totalExperience = 0;

	// DO NOT GET DIRECTLY, USE GETTER
	private static Dictionary<int, int> levelXpThresholds = new Dictionary<int, int> ();

	private static void LoadLevelXpThresholds () {
		// TODO make a csv for this and load that
		levelXpThresholds.Add (1, 0);
		levelXpThresholds.Add (2, 100);
		levelXpThresholds.Add (3, 250);
		levelXpThresholds.Add (4, 500);
		levelXpThresholds.Add (5, 1000);
		levelXpThresholds.Add (6, 2500);
		levelXpThresholds.Add (7, 1000000);
	}

	public static int GetLevelXpThreshold (int level) {
		if (levelXpThresholds.Count == 0) {
			LoadLevelXpThresholds ();
		}

		return levelXpThresholds[level];
	}

	public static bool AddExperience (int xp) {
		bool isLevelUp = false;

		totalExperience += xp;
		if (totalExperience >= GetNextLevelThreshold ()) {
			playerLevel++;
			isLevelUp = true;
		}

		return isLevelUp;
	}

	public static int GetPlayerLevel () {
		return playerLevel;
	}

	public static int GetTotalExperience () {
		return totalExperience;
	}

	public static int GetXpToNextLevel () {
		int nextLevel = playerLevel + 1;
		int xpThreshold = GetLevelXpThreshold (nextLevel);

		return xpThreshold - totalExperience;
	}

	public static int GetNextLevelThreshold () {
		return GetLevelXpThreshold (playerLevel + 1);
	}

}