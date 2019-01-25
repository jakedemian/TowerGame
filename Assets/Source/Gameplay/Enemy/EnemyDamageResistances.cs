using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageResistances : MonoBehaviour {
	[Range (0f, 1f)]
	public float physicalDamageResistance;

	[Range (0f, 1f)]
	public float magicDamageResistance;

	[Range (0f, 1f)]
	public float fireDamageResistance;

	[Range (0f, 1f)]
	public float frostDamageResistance;

	public float GetDamageAfterResistances (float physicalDmg, float fireDmg, float frostDmg) {
		float adjustedPhysicalDmg = physicalDmg - (physicalDmg * physicalDamageResistance);
		float adjustedFireDmg = fireDmg - (fireDmg * magicDamageResistance) - (fireDmg * fireDamageResistance);
		if (adjustedFireDmg < 0f) {
			adjustedFireDmg = 0f;
		}
		float adjustedFrostDmg = frostDmg - (frostDmg * magicDamageResistance) - (frostDmg * frostDamageResistance);
		if (adjustedFrostDmg < 0f) {
			adjustedFrostDmg = 0f;
		}

		return adjustedPhysicalDmg + adjustedFireDmg + adjustedFrostDmg;
	}

	public float GetDamageAfterResistances (Damage damage) {
		return GetDamageAfterResistances (damage.GetPhysicalDamage (), damage.GetFireDamage (), damage.GetFrostDamage ());
	}
}