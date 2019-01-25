using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SkillBaseData {
	// TODO have this loaded dynamically from a JSON or XML file
	private static Dictionary<string, SkillBase> skillBaseData = new Dictionary<string, SkillBase> ();
	private static Dictionary<string, int> skillLevelData = new Dictionary<string, int> ();

	public static SkillBase GetSkillBaseData (string name) {
		if (skillBaseData.Count == 0) {
			PopulateSkillBaseData ();
		}

		return skillBaseData[name];
	}

	private static void PopulateSkillBaseData () {
		// TODO check if we have any saved skill data.  
		// otherwise, load all new data like now

		for (int i = 0; i < SkillNames.SKILL_NAMES.Length; i++) {
			// first, init our level data for this skill to level 1
			skillLevelData.Add (SkillNames.SKILL_NAMES[i], 1);

			// then, load that level's data into the current stored data
			string skillName = SkillNames.SKILL_NAMES[i];
			Debug.Log ("loading skill " + skillName);

			TextAsset csv = Resources.Load<TextAsset> ("SkillData/" + skillName);
			string[] line = CSVReader.ReadCsvRow (csv, 1);
			skillBaseData.Add (
				skillName,
				new SkillBase (
					skillName,
					float.Parse (line[5]),
					float.Parse (line[1]),
					float.Parse (line[2]),
					float.Parse (line[3]),
					float.Parse (line[4]),
					float.Parse (line[6]),
					float.Parse (line[7]),
					float.Parse (line[8])));
		}
	}

	public static void NotifySkillLevelUp (string skillName) {
		// TODO check if we have level data for this skill, and if not, look for it stored in a file
		// if none is found, assume level 1, then level up to 2

		// level the skill up
		skillLevelData[skillName] = skillLevelData[skillName] + 1;
		SetSkillLevel (skillName, skillLevelData[skillName]);

	}

	public static void SetSkillLevel (string skillName, int level) {
		// verify we have skill data
		if (!skillBaseData.ContainsKey (skillName)) {
			Debug.LogError ("We don't have any data for the skill: " + skillName);
			return;
		}

		TextAsset csv = Resources.Load<TextAsset> ("SkillData/" + skillName);
		string[] line = CSVReader.ReadCsvRow (csv, level);
		skillBaseData[skillName] = new SkillBase (
			skillName,
			float.Parse (line[5]),
			float.Parse (line[1]),
			float.Parse (line[2]),
			float.Parse (line[3]),
			float.Parse (line[4]),
			float.Parse (line[6]),
			float.Parse (line[7]),
			float.Parse (line[8]));
	}
}

public class SkillBase {
	public SkillBase (string n, float cd, float pdmg, float fidmg, float frdmg, float r, float pv, float s, float mc) {
		name = n;
		cooldown = cd;
		physicalDmg = pdmg;
		fireDmg = fidmg;
		frostDmg = frdmg;
		radius = r;
		physicalVariance = pv;
		shield = s;
		manaCost = mc;
	}

	public string name;
	public int targetMode;
	public float cooldown;
	public float physicalDmg;
	public float fireDmg;
	public float frostDmg;
	public float radius;
	public float physicalVariance;
	public float shield;
	public float manaCost;
}

public static class SkillNames {
	public static string CANNON = "CANNONBALL";
	public static string MYSTIC_BULWARK = "MYSTIC_BULWARK";

	public static string[] SKILL_NAMES = {
		CANNON,
		MYSTIC_BULWARK
	};
}