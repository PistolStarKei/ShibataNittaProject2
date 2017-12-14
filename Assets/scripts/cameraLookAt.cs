using UnityEngine;
using System.Collections;

public class cameraLookAt : MonoBehaviour {

	public Transform target;
	private Vector3 positionOffset;		// 相対座標

	// Use this for initialization
	void Start () {
		positionOffset = GetComponent<Transform>().position - target.position;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Transform>().position = target.position + positionOffset;

//		transform.LookAt(target.transform);
//		Debug.Log("tmpPos == " + target.transform.position , this);
	}
}
