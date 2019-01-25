using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillbarUIController : MonoBehaviour {
	private List<GameObject> skillPanels = new List<GameObject> ();
	private List<GameObject> cooldownPanels = new List<GameObject> ();
	private float skillPanelStartingHeight = -1f;
	private float skillPanelStartingWidth = -1f;

	private List<CooldownTime> cooldownTimes = new List<CooldownTime> ();

	void Start () {
		PopulateSkillPanels ();

		for (int i = 0; i < skillPanels.Count; i++) {
			cooldownPanels.Add (skillPanels[i].transform.GetChild (2).gameObject);
			SetSkillPanelHeight (i, 0f); // start them at 0
		}

		for (int i = 0; i < skillPanels.Count; i++) {
			cooldownTimes.Add (new CooldownTime (0f, 0f));
		}
	}

	void Update () {
		for (int i = 0; i < cooldownTimes.Count; i++) {
			if (cooldownTimes[i].cooldown != 0f && cooldownTimes[i].timer > 0f) {
				cooldownTimes[i].timer -= Time.deltaTime;
				if (cooldownTimes[i].timer < 0f) {
					cooldownTimes[i].timer = 0f;
				}

				// update cooldown panel height
				float percentToGo = cooldownTimes[i].timer / cooldownTimes[i].cooldown;
				SetSkillPanelHeight (i, skillPanelStartingHeight * percentToGo);
			}
		}
	}

	private void PopulateSkillPanels () {
		for (int i = 0; i < transform.childCount; i++) {
			skillPanels.Add (transform.GetChild (i).gameObject);
		}
	}

	private void SetSkillPanelHeight (int index, float height) {
		if (skillPanelStartingHeight < 0f || skillPanelStartingWidth < 0f) {
			skillPanelStartingHeight = cooldownPanels[0].GetComponent<RectTransform> ().rect.height;
			skillPanelStartingWidth = cooldownPanels[0].GetComponent<RectTransform> ().rect.width;
		}

		cooldownPanels[index].GetComponent<RectTransform> ().sizeDelta = new Vector2 (skillPanelStartingWidth, height);
	}

	public void StartSkillCooldownUI (int skillIndex, float cooldownTime) {
		cooldownTimes[skillIndex].timer = cooldownTime;
		cooldownTimes[skillIndex].cooldown = cooldownTime;
		SetSkillPanelHeight (skillIndex, skillPanelStartingHeight);
	}

}

public class CooldownTime {
	public CooldownTime (float t, float cd) {
		timer = t;
		cooldown = cd;
	}
	public float timer;
	public float cooldown;
}

public static class SkillbarUI {
	private static SkillbarUIController GetController () {
		return GameObject.FindGameObjectWithTag ("SkillsUI").GetComponent<SkillbarUIController> ();
	}

	public static void UpdateSkillCooldownUI (int index, float cooldown) {
		GetController ().StartSkillCooldownUI (index, cooldown);
	}
}