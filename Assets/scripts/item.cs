using UnityEngine;
using System.Collections;

public class item : MonoBehaviour {

	public AudioClip sound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Destroy(this.gameObject);
		AudioSource.PlayClipAtPoint(sound, transform.position);
	}
}
