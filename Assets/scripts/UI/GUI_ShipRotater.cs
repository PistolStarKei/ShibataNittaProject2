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
		originalPosition=transform.localPosition;
		isInited=true;
	}
	public void SetToDefault(){
		if(!isInited)Init();
		transform.rotation=originalRot ;
		transform.localPosition=originalPosition;
		degrees=0.0f;
		degrees2=0.0f;
		isPressing=false;
	}

	float degrees;
	float degrees2;
	// Update is called once per frame
	void Update () {
		if(!isInited)Init();
		if(isPressing)return;
		degrees+=rotateSpeedForward*Time.deltaTime;

		degrees2+=rotateSpeedUp*Time.deltaTime;

		//transform.rotation = originalRot * Quaternion.AngleAxis(degrees, Vector3.forward)* Quaternion.AngleAxis(degrees2, Vector3.up);

		transform.rotation = Quaternion.Slerp( transform.rotation, originalRot * Quaternion.AngleAxis(degrees, Vector3.forward)* Quaternion.AngleAxis(degrees2, Vector3.up), Time.deltaTime );

	}
	bool isPressing=false;
	public void OnPress(bool isPress,Vector3 vec){
		isPressing=isPress;
		LookAt(vec);
	}
	public void OnTapUpdate(Vector3 vec){
		LookAt(vec);
	}

	Vector3 target;
	public void LookAt(Vector3 vec){
		target=vec;
		transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( target - transform.position ), Time.deltaTime );
	}

}
