using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
	private List<GameObject> pool = new List<GameObject> ();

	public GameObject Instantiate (GameObject prefab, Vector3 loc, Quaternion rot) {
		GameObject newGameObject = null;

		for (int i = 0; i < pool.Count; i++) {
			if (prefab.tag == pool[i].tag && !pool[i].activeInHierarchy) {
				newGameObject = pool[i];
			}
		}

		if (newGameObject == null) {
			newGameObject = Object.Instantiate (prefab, loc, rot);

			// set parent to self (so pooled objects are hidden under ObjectPool obj in scene)
			newGameObject.transform.parent = gameObject.transform;

			pool.Add (newGameObject);
		}

		// need to reaffirm location, rot, etc because if this already existed it will not be correct
		newGameObject.transform.position = loc;
		newGameObject.transform.rotation = rot;

		if (newGameObject.GetComponent<Rigidbody> () != null) {
			newGameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			newGameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		}

		newGameObject.SetActive (true);
		return newGameObject;
	}

	public void Destroy (GameObject go) {
		int objId = go.GetInstanceID ();
		for (int i = 0; i < pool.Count; i++) {
			if (pool[i].GetInstanceID () == objId) {
				pool[i].SetActive (false);
				return;
			}
		}
	}
}

public static class ObjectPoolHelper {
	private static ObjectPool GetObjectPool () {
		return GameObject.FindGameObjectWithTag ("ObjectPool").GetComponent<ObjectPool> ();
	}

	public static GameObject Instantiate (GameObject go, Vector3 pos, Quaternion rot) {
		return GetObjectPool ().Instantiate (go, pos, rot);
	}

	public static void Destroy (GameObject go) {
		GetObjectPool ().Destroy (go);
	}
}