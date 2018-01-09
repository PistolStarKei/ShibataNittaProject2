using UnityEngine;
using System.Collections;

public class shot : MonoBehaviour {

	private int life = 60;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate( Vector3.forward * 0.05f);

		life--;
		if (life <= 0) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer == LayerMask.NameToLayer("enemy")){
			Destroy(this.gameObject);
			AudioController.Play ("Explosion");
		}

		if(other.gameObject.layer == LayerMask.NameToLayer("Wall")){
			Destroy(this.gameObject);
		}
	}
}
