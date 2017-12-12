using UnityEngine;
using System.Collections;

public class cameraLookAt : MonoBehaviour {

	public Transform target;
	private Vector3 offset;		// 相対座標

	// Use this for initialization
	void Start () {
		offset = GetComponent<Transform>().position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Transform>().position = target.position + offset;

//		transform.LookAt(target.transform);
//		Debug.Log("tmpPos == " + target.transform.position , this);
	}
}
