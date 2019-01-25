using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour {
	public GameObject pScout;
	public GameObject pCatapult;

	private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject> ();
	public float scoutSpawnRate;
	public float catapultSpawnRate;
	public GameObject leftGroundSpawn;
	public GameObject rightGroundSpawn;

	private GameController gameController;
	private float scoutSpawnTimer = 0f;
	private float catapultSpawnTimer = 0f;

	/**
		TTTTTT    OOOOOO    DDDD     OOOOOO
		  TT      O    O    D    D   O    O
		  TT      O    O    D    D   O    O
		  TT      O    O    D    D   O    O
		  TT      OOOOOO    DDDD     OOOOOO   

		I need to design exactly how I want levels and enemy spawns to work.  There's a bunch of ways I can think of:

		1)Each level has a set order of enemy spawns and times, listed out in a csv.
		2)Each level has a set pool of enemies that have to be spawned, but they can be done in any order at varying times.
		3)Each level has a set of enemies that CAN spawn, and a spawn "chance" (that always adds up to 100%), and each level
			will vary in which enemies spawn.

		I also need to work out spawn times.  Do I want enemies spawning on a fixed interval?  Maybe I should have a spawn
			tick rate and each tick has a % chance to spawn an enemy, with higher levels having higher %'s.  Then, once you
			say "okay, this tick we are spawning an enemy", you continue with the selecting of which enemy to spawn.  The
			drawbacks of this are a level could theoretically be over in like 5 seconds, but the same level next try could
			also take 10 minutes.
	 */
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		InitEnemyPrefabsDictionary ();
	}

	private void InitEnemyPrefabsDictionary () {
		enemyPrefabs.Add ("SCOUT", pScout);
		enemyPrefabs.Add ("CATAPULT", pCatapult);
	}

	void Update () {
		if (scoutSpawnTimer <= 0f) {
			scoutSpawnTimer = scoutSpawnRate;
			SpawnEnemy (pScout);
		}

		if (catapultSpawnTimer <= 0f) {
			catapultSpawnTimer = catapultSpawnRate;
			SpawnEnemy (pCatapult);
		}

		scoutSpawnTimer -= Time.deltaTime;
		catapultSpawnTimer -= Time.deltaTime;
	}

	private void SpawnEnemy (GameObject enemyPrefab) {
		float spawnPointX = Random.Range (
			leftGroundSpawn.transform.position.x,
			rightGroundSpawn.transform.position.x);

		GameObject newEnemy = ObjectPoolHelper.Instantiate (
			enemyPrefab,
			new Vector3 (spawnPointX, 0.2f, leftGroundSpawn.transform.position.z),
			Quaternion.identity);

		gameController.NofityNewEnemySpawn (newEnemy);
	}
}