using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActionController : MonoBehaviour {

	private GameController gameController;
	public static Dictionary<string, float> skillCooldowns = new Dictionary<string, float> ();

	public GameObject fort;
	public GameObject pCannonball;
	public GameObject pGroundParticle;

	private void Start () {
		gameController = GetComponent<GameController> ();
	}

	private void Update () {
		UpdateSkillCooldowns ();
	}

	private void UpdateSkillCooldowns () {
		List<string> skillCooldownKeys = new List<string> (skillCooldowns.Keys);

		for (int i = 0; i < skillCooldownKeys.Count; i++) {
			string key = skillCooldownKeys[i];
			if (skillCooldowns[key] > 0) {
				skillCooldowns[key] -= Time.deltaTime;
			}
		}
	}

	private void StartSkillCooldown (string skillName, float cd, int skillbarIndex) {
		if (!skillCooldowns.ContainsKey (skillName)) {
			skillCooldowns.Add (skillName, 0);
		}

		skillCooldowns[skillName] = cd;

		SkillbarUI.UpdateSkillCooldownUI (skillbarIndex, cd);
	}

	public void executeSkill (SkillBase skillBase, int skillbarIndex) {
		string skillName = skillBase.name;

		// check if skill is off cooldown
		if (skillCooldowns.ContainsKey (skillName) && skillCooldowns[skillName] > 0f) {
			Debug.Log (skillBase.name + " is not off cooldown yet.");
			return;
		}

		// check if we have the mana for this skill
		if (!ManaHelper.HasEnoughMana (skillBase.manaCost)) {
			Debug.Log ("Not enough mana to cast " + skillBase.name);
			return;
		}

		if (skillName == SkillNames.CANNON) {
			// fetch the target position
			Vector3 target = GetComponent<TargetingModeController> ().GetGroundTargetPosition ();
			bool successfulShot = ShootCannonball (target);

			if (successfulShot) {
				StartSkillCooldown (skillName, skillBase.cooldown, skillbarIndex);
			}
		} else if (skillName == SkillNames.MYSTIC_BULWARK) {
			ManaHelper.SpendMana (skillBase.manaCost);
			StartSkillCooldown (skillName, skillBase.cooldown, skillbarIndex);

			ShieldHelper.SetShield (skillBase.shield);
		}
	}

	/*
	 *	CANNONBALL
	 */
	private bool ShootCannonball (Vector3 target) {
		// verify valid target
		if (target.y < 0) {
			Debug.LogWarning ("Cannot shoot at the target location.");
			return false;
		}

		GameObject cannonball = ObjectPoolHelper.Instantiate (
			pCannonball,
			fort.transform.position + new Vector3 (0, 1.2f, 0),
			Quaternion.identity);

		cannonball.GetComponent<CannonballController> ().Init (target, false, 0f);
		return true;
	}

	public void CallbackCannonballHit (Vector3 location) {
		SkillBase skillData = SkillBaseData.GetSkillBaseData (SkillNames.CANNON);
		float radius = skillData.radius;

		// TODO FIXME so do we want it to vary per cannonball, or per enemy hit?
		// my initiol reaction is it should be that the physical damage done to each
		// enemy is what varies, even within a single cannonball shot
		gameController.DoAOEGroundDamage (location, radius, skillData);

		// GENERATE PARTICLES :) (needs to me moved / reworked)
		// lots of optimization to happen here
		int numOfParticles = 40;
		for (int i = 0; i < numOfParticles; i++) {
			// create ground particles
			Vector2 particleCircle = Random.insideUnitCircle * radius;
			float particleX = location.x + particleCircle.x;
			float particleZ = location.z + particleCircle.y;
			GameObject particle = ObjectPoolHelper.Instantiate (pGroundParticle, new Vector3 (particleX, 0, particleZ), Quaternion.identity);
			particle.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 3, 0);

			float spin = 100f;
			particle.GetComponent<Rigidbody> ().AddTorque (new Vector3 (Random.Range (-spin, spin), Random.Range (-spin, spin), Random.Range (-spin, spin)));
		}
	}
}