using UnityEngine;
using System.Collections;

public class cameraLookAt : MonoBehaviour {

	public Transform target;

	Transform tr;
	Vector3 vec=Vector3.zero;
	public float offset=4.0f;

	// Use this for initialization
	void Start () {
		
		tr=GetComponent<Transform>();
	}


	// Update is called once per frame
	void Update () {
		
		vec=target.position;
		vec.y=target.position.y+offset;
		if(tr!=null)tr.position = vec;

	}
}
