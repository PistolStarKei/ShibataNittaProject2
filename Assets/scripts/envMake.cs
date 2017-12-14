using UnityEngine;
using System.Collections;

public class envMake : MonoBehaviour {

	public GameObject asteroid;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 100; i++) {
			Quaternion rot = Quaternion.Euler (0, Random.Range(0, 360), 0);
			Vector3 pos = new Vector3 (Random.Range (-10, 10), 0, Random.Range (-10, 10));
			Instantiate (asteroid, pos, rot);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
