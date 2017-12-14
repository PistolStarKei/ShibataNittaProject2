using UnityEngine;
using System.Collections;

public class shipControl : MonoBehaviour {

	Quaternion from;
	Quaternion to;

	Vector3 newRotation = new Vector3(0,0,0);

	public GameObject shot;

	IEnumerator Start ()
	{
		while (true) {
			// 弾をプレイヤーと同じ位置/角度で作成
			Vector3 temp = transform.forward * 0.2f;

			Instantiate (shot, transform.position + temp, transform.rotation);
			// 0.1秒待つ
			yield return new WaitForSeconds (0.2f);
		}
	}

	// 移動スピード
	public float speed = 0.1f;

	void Update () {
//		keyControl ();

		if( TapEffect.tapFlag ){
			tapControl ();
		}

		// 回転
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * 2.0f);
		// 移動
		transform.Translate( Vector3.forward * 0.01f);
	}

	void tapControl(){
		// タップの方向に向く
		Vector3 temp = TapEffect.tapPos;
		temp.x = temp.x;
		temp.y = 0.0f;
		temp.z = temp.z;

		newRotation = Quaternion.LookRotation(temp - transform.position).eulerAngles;
		newRotation.x = 0;
		newRotation.z = 0;
	}

	void keyControl(){
		float x = Input.GetAxisRaw ("Horizontal");		// 右・左
		float z = Input.GetAxisRaw ("Vertical");		// 上・下
		Vector3 direction = new Vector3 (x, 0.0f, z).normalized;		// 移動する向きを求める

		// 移動する向きとスピードを代入する
		transform.position += direction * speed;
	}
}
