using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadoutsMenuController : MonoBehaviour {
	public void GoToTowerDefenseScene () {
		SceneManager.LoadScene ("TowerDefense");
	}
}