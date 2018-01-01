using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

	public GameObject explosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Destroy(this.gameObject);
		Instantiate (explosion, transform.position, transform.rotation);
	}
}
