using UnityEngine;
using System.Collections;

public class shipControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	// 移動スピード
	public float speed = 3;
	private GameObject _child;
	
	// Update is called once per frame
	void Update () {
		// 右・左
		float x = Input.GetAxisRaw ("Horizontal");

		// 上・下
		float z = Input.GetAxisRaw ("Vertical");

		// 移動する向きを求める
		Vector3 direction = new Vector3 (x, 0.0f, z).normalized;

		// 移動する向きとスピードを代入する
		_child = transform.FindChild ("spaceShip").gameObject;
		_child.GetComponent<Rigidbody>().velocity = direction * speed;
	}
}
