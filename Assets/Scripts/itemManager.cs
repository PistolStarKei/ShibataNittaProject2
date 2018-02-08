using UnityEngine;
using System.Collections;

public class itemManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		checkSpawnPrefab ();
		checkItemPrefab ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// スポーンポイントになるプレハブがあるか調べる
	private void checkSpawnPrefab(){

		GameObject spawnPoints = GameObject.Find ("SpawnPoints");
		while (true) {
			GameObject obj = GameObject.Find ("mapManager/stage00/SpawnPoint");

			if (obj != null) {
				// あったらSpawnPointsの子供に接続
				obj.transform.parent = null;
				obj.transform.parent = spawnPoints.transform;
			} else {
				break;
			}
		}
	}

	// アイテムポイントになるプレハブがあるか調べる
	private void checkItemPrefab(){

		GameObject spawnBounds = GameObject.Find ("SpawnBounds");
		while (true) {
			GameObject obj = GameObject.Find ("mapManager/stage00/bounds");

			if (obj != null) {
				// あったらSpawnPointsの子供に接続
				obj.transform.parent = null;
				obj.transform.parent = spawnBounds.transform;
			} else {
				break;
			}
		}

	}
}
