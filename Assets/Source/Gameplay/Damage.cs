public class Damage {
	private float physicalDamage;
	private float fireDamage;
	private float frostDamage;

	public Damage (float p, float fi, float fr) {
		physicalDamage = p;
		fireDamage = fi;
		frostDamage = fr;
	}

	public float GetDamage () {
		return physicalDamage + fireDamage + frostDamage;
	}

	public float GetPhysicalDamage () {
		return physicalDamage;
	}

	public float GetMagicDamage () {
		return fireDamage + frostDamage;
	}

	public float GetFireDamage () {
		return fireDamage;
	}

	public float GetFrostDamage () {
		return frostDamage;
	}

}