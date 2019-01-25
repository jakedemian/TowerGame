using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	private List<string> currentSkills = new List<string> () { SkillNames.CANNON, SkillNames.MYSTIC_BULWARK, "", "", "" };
	private List<KeyCode> skillKeybinds = new List<KeyCode> () {
		KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.T
	};
	private List<GameObject> currentEnemies = new List<GameObject> ();

	void Update () {
		CheckSkillKeyPress ();
	}

	private void CheckSkillKeyPress () {
		for (int i = 0; i < skillKeybinds.Count; i++) {
			if (Input.GetKeyDown (skillKeybinds[i])) {
				if (currentSkills[i] == "") {
					Debug.Log ("There is no skill bound to this key right now.");
					continue;
				}

				string pressedSkillName = currentSkills[i];
				Debug.Log (pressedSkillName);
				SkillBase skillBase = SkillBaseData.GetSkillBaseData (pressedSkillName);
				GetComponent<SkillActionController> ().executeSkill (skillBase, i);
			}
		}
	}

	public void DoAOEGroundDamage (Vector3 loc, float radius, SkillBase skillData) {
		for (int i = 0; i < currentEnemies.Count; i++) {
			if (currentEnemies[i] != null && currentEnemies[i].activeInHierarchy) {
				// TODO FIXME need a grounded check here.  an enemy that doesnt take ground damage
				// should not be hit by this!!!

				// check if theyre in range, and if so, deal damage to them
				Vector2 enemyLoc2D = new Vector2 (
					currentEnemies[i].transform.position.x,
					currentEnemies[i].transform.position.z);

				Vector2 impactLoc2D = new Vector2 (loc.x, loc.z);

				if (Vector2.Distance (enemyLoc2D, impactLoc2D) <= radius) {
					// TODO process player buffs and debuffs here!!!

					// Add variance to this hit
					Damage damage = GenerateDamageVariance (skillData);

					currentEnemies[i].GetComponent<EnemyHealthController> ().DealDamage (damage);
				}
			}
		}
	}

	private Damage GenerateDamageVariance (SkillBase skillData) {
		float physicalDamage = skillData.physicalDmg;
		float physicalVariance = skillData.physicalVariance;
		float finalPhysicalDmg = Random.Range (physicalDamage - physicalVariance, physicalDamage + physicalVariance);

		return new Damage (finalPhysicalDmg, skillData.fireDmg, skillData.frostDmg);
	}

	public void NofityNewEnemySpawn (GameObject spawn) {
		// don't add a pooled enemy that's already in the list
		for (int i = 0; i < currentEnemies.Count; i++) {
			if (currentEnemies[i].GetInstanceID () == spawn.GetInstanceID ()) {
				return;
			}
		}

		currentEnemies.Add (spawn);
	}
}