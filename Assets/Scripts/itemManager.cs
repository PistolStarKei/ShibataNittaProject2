using UnityEngine;
using System.Collections;

public class itemManager : MonoBehaviour {

	// スポーンポイントになるプレハブがあるか調べる
	public void checkSpawnPrefab(){

		GameObject spawnPoints = GameObject.Find ("SpawnPoints");
		GameObject mapManager = GameObject.Find ("mapManager");
		Transform[] transformArray = mapManager.GetComponentsInChildren<Transform> ();

		foreach (Transform child in transformArray) {
			if (child.name == "SpawnPoint") {
				child.parent = spawnPoints.transform;
			}
		}
	}

	// アイテムポイントになるプレハブがあるか調べる
	public void checkItemPrefab(){

		GameObject spawnBounds = GameObject.Find ("SpawnBounds");
		GameObject mapManager = GameObject.Find ("mapManager");
		Transform[] transformArray = mapManager.GetComponentsInChildren<Transform> ();

		foreach (Transform child in transformArray) {
			if (child.name == "bounds") {
				child.parent = spawnBounds.transform;
			}
		}
	}
}
