using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

	public GameObject explosion;
	public GameObject shot;

	// Use this for initialization
	void Start () {
		StartCoroutine(ShotCoroutine());
	}

	Vector3 temp;
	public float shotDulation=0.2f;
	public float shotOffset=0.2f;
	IEnumerator ShotCoroutine ()
	{
		while (true) {
			// 弾をプレイヤーと同じ位置/角度で作成
			temp = transform.forward *shotOffset;

			Instantiate (shot, transform.position + temp, transform.rotation);
			// 0.1秒待つ
			yield return new WaitForSeconds (shotDulation);
		}
	}


	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer==LayerMask.NameToLayer("Shot")){
			Destroy(this.gameObject);
			Instantiate (explosion, transform.position, transform.rotation);
			AudioController.Play ("Explosion2");
		}
	}
}
