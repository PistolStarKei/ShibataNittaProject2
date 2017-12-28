using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.green);
	}
}
