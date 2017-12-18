using UnityEngine;
using System.Collections;

public class envMake : MonoBehaviour {

	public GameObject asteroid;
	public GameObject enemy;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 100; i++) {
			Quaternion rot = Quaternion.Euler (0, Random.Range(0, 360), 0);
			Vector3 pos = new Vector3 (Random.Range (-15, 15), 0, Random.Range (-15, 15));
			Instantiate (asteroid, pos, rot);
		}

		for (int i = 0; i < 100; i++) {
			Quaternion rot = Quaternion.Euler (0, Random.Range(0, 360), 0);
			Vector3 pos = new Vector3 (Random.Range (-15, 15), 0, Random.Range (-15, 15));
			Instantiate (enemy, pos, rot);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
