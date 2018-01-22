using UnityEngine;
using System.Collections;

public class envMake : MonoBehaviour {

	public GameObject asteroid;
	public GameObject enemy;
	public GameObject pick_repair1;
	public GameObject pick_repair2;
	public GameObject pick_repair3;

	public GameObject pick_napam;
	public GameObject pick_allway;
	public GameObject pick_laser;
	public GameObject pick_wave;
	public GameObject pick_homing;
	public GameObject pick_stealth;
	public GameObject pick_nuke;

	// Use this for initialization
	void Start () {
		// 岩
		for (int i = 0; i < 30; i++) {
			Quaternion rot = Quaternion.Euler (0, Random.Range(0, 360), 0);
			Vector3 pos = new Vector3 (Random.Range (-15, 15), 0, Random.Range (-15, 15));
			Instantiate (asteroid, pos, rot);
		}

		// 敵
		for (int i = 0; i < 100; i++) {
			Quaternion rot = Quaternion.Euler (0, Random.Range(0, 360), 0);
			Vector3 pos = new Vector3 (Random.Range (-15, 15), 0, Random.Range (-15, 15));
			Instantiate (enemy, pos, rot);
		}

		// 回復アイテム
		for (int i = 0; i < 100; i++) {
			Quaternion rot = Quaternion.Euler (0, Random.Range(0, 360), 0);
			Vector3 pos = new Vector3 (Random.Range (-15, 15), 0, Random.Range (-15, 15));
			int rnd = Random.Range (0, 2);
			if (rnd == 0) {
				Instantiate (pick_repair1, pos, rot);
			} else if (rnd == 1) {
				Instantiate (pick_repair2, pos, rot);
			} else if (rnd == 2) {
				Instantiate (pick_repair3, pos, rot);
			}
		}

		// パワーアップアイテム
//		for (int i = 0; i < 100; i++) {
//			Quaternion rot = Quaternion.Euler (0, Random.Range(0, 360), 0);
//			Vector3 pos = new Vector3 (Random.Range (-15, 15), 0, Random.Range (-15, 15));
//			Instantiate (enemy, pos, rot);
//		}

	}
	
	// Update is called once per frame
	void Update () {
	}
}
