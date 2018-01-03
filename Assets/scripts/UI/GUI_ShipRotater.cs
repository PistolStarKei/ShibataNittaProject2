using UnityEngine;
using System.Collections;

public class GUI_ShipRotater : MonoBehaviour {


	public float rotateSpeedForward=0.1f;
	public float rotateSpeedUp=0.1f;

	Quaternion originalRot;
	Vector3 originalPosition;

	bool isInited=false;
	void Init(){
		originalRot = transform.rotation;
		originalPosition=transform.position;
		isInited=true;
	}
	public void SetToDefault(){
		if(!isInited)Init();
		transform.rotation=originalRot ;
		transform.position=originalPosition;
		degrees=0.0f;
		degrees2=0.0f;
	}

	float degrees;
	float degrees2;
	// Update is called once per frame
	void Update () {
		if(!isInited)Init();

		degrees+=rotateSpeedForward*Time.deltaTime;

		degrees2+=rotateSpeedUp*Time.deltaTime;

		transform.rotation = originalRot * Quaternion.AngleAxis(degrees, Vector3.forward)* Quaternion.AngleAxis(degrees2, Vector3.up);


	}
}
