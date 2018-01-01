using UnityEngine;
using System.Collections;

public class shipControl : MonoBehaviour {

	Vector3 newRotation = new Vector3(0,0,0);
	public GameObject shot;


	void Start(){

		//カメラに機体を設定
		Camera.main.gameObject.GetComponent<cameraLookAt>().target=this.gameObject.transform;
		//GUIManagerに機体を設定
		GUIManager.Instance.shipControll=this;

		StartCoroutine(ShotCoroutine());
		isPressed=false;
	}

	IEnumerator ShotCoroutine ()
	{
		while (true) {
			// 弾をプレイヤーと同じ位置/角度で作成
			Vector3 temp = transform.forward * 0.2f;

			Instantiate (shot, transform.position + temp, transform.rotation);
			// 0.1秒待つ
			yield return new WaitForSeconds (0.2f);
		}
	}


	//GUIManagerからの入力受け取りメソッド

	public void OnPressTapLayer(bool isPress,Vector3 worldPos){
		//Debug.Log("OnPressTapLayer"+isPress+worldPos.ToString());
		isPressed=isPress;
		testTrans.position=worldPos;
	}

	public void OnUpdateTapLayer(Vector3 worldPos){
		currentTappedPos=worldPos;
		testTrans.position=worldPos;
	}

	public bool isPressed=false;
	Vector3 currentTappedPos;
	public Transform testTrans;

	// 移動スピード
	public const float maxSpeed = 1.0f;
	public float speed = 0.01f;
	public int speedCount = 0;

		void Update () {
		Vector3 tr = transform.position.normalized;
		Rigidbody rd = GetComponent<Rigidbody> (); 

		if (isPressed) {
			tapControl ();

			// 回転
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (newRotation), Time.deltaTime * 4.0f);
			// 移動

		} else {
			// 徐々に止める
			Vector3 velocity = new Vector3(rd.velocity.x, 0, rd.velocity.z);
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

	void tapControl(){
		// タップの方向に向く
		Vector3 temp = currentTappedPos;
		temp.x = temp.x;
		temp.y = 0.0f;
		temp.z = temp.z;

		newRotation = Quaternion.LookRotation(temp - transform.position).eulerAngles;
		newRotation.x = 0;
		newRotation.z = 0;
	}

}
