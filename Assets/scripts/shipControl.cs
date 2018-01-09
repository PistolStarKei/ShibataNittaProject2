using UnityEngine;
using System.Collections;

public class shipControl : MonoBehaviour {

	Vector3 newRotation = new Vector3(0,0,0);
	public GameObject explosion;
	public GameObject shot;

	public bool isOwnerShip=false;
	void Start(){

		//GUIManagerに機体を設定
		if(isOwnerShip)GUIManager.Instance.SetShipControll(this);

		rd = GetComponent<Rigidbody> (); 

		gameObject.tag="Player";

		StartCoroutine(ShotCoroutine());
		isPressed=false;
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


	//GUIManagerからの入力受け取りメソッド

	public void OnPressTapLayer(bool isPress,Vector3 worldPos){
		//Debug.Log("OnPressTapLayer"+isPress+worldPos.ToString());
		isPressed=isPress;
		currentTappedPos=worldPos;
	}

	public void OnUpdateTapLayer(Vector3 worldPos){
		//Debug.Log("OnPressTapLayer"+worldPos.ToString());
		currentTappedPos=worldPos;
	}


	public bool isPressed=false;
	Vector3 currentTappedPos;

	// 移動スピード
	public const float maxSpeed = 2.0f;
	public float speed = 0.01f;
	public int speedCount = 0;

	Vector3 tr;
	Rigidbody rd;
	Vector3 velocity;

	void Update () {
		
		tr = transform.position.normalized;

		if (isPressed) {
			// タップの方向に向く
			newRotation = Quaternion.LookRotation(currentTappedPos - transform.position).eulerAngles;
			newRotation.x = 0;
			newRotation.z = 0;

			// 回転
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (newRotation), Time.deltaTime * 4.0f);

		} else {

			// 徐々に止める
			velocity = new Vector3(rd.velocity.x, 0, rd.velocity.z);
			velocity = velocity - (velocity / 40);
			rd.velocity = velocity;
		}



		if(rd.velocity.magnitude > maxSpeed)
		{
			rd.velocity = rd.velocity.normalized * maxSpeed;
		}

		tr = transform.forward * speed;

		rd.AddForce (tr, ForceMode.VelocityChange);
	}

	// 敵の弾に当たった場合
	void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == LayerMask.NameToLayer("Shot")){
//			Destroy(this.gameObject);


			Instantiate (explosion, transform.position, transform.rotation);
			AudioController.Play ("Explosion2");

			GUIManager.Instance.Damage (100.0f, 1500.0f);
		}

		if(other.gameObject.layer == LayerMask.NameToLayer("PickUp")){
			Pickup putype=other.gameObject.GetComponent<item>().pickType;


			switch(putype){
				case Pickup.CureS:
					AudioController.Play ("Powerup");
					GUIManager.Instance.Cure (30.0f, 1500.0f);
					break;
				case Pickup.CureM:
					AudioController.Play ("Powerup");
					GUIManager.Instance.Cure (30.0f, 1500.0f);
					break;
			}

			Destroy(other.gameObject);
		}
	}
}
