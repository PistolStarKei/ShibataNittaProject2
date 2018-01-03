using UnityEngine;
using System.Collections;

public class GUI_ShipRotater : MonoBehaviour {


	public float rotateSpeedForward=0.1f;
	public float rotateSpeedUp=0.1f;
	Quaternion originalRot;
	void Start(){
		originalRot = transform.rotation;
	}
	float degrees;
	float degrees2;
	// Update is called once per frame
	void Update () {
		
		degrees+=rotateSpeedForward*Time.deltaTime;

		degrees2+=rotateSpeedUp*Time.deltaTime;

		transform.rotation = originalRot * Quaternion.AngleAxis(degrees, Vector3.forward)* Quaternion.AngleAxis(degrees2, Vector3.up);


	}
}
