using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// PURotaterの説明
/// </summary>
public class PURotater : MonoBehaviour {

	#region  メンバ変数
	public float rotateSpeed=0.1f;
	Quaternion originalRot;
	public Vector3 axis;
	#endregion
	void Start(){
		originalRot = transform.rotation;
	}

	#region  Update
	float degrees;
	float degrees2;
	float degrees3;
	void Update(){
		degrees+=rotateSpeed*Time.deltaTime;

		transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.AngleAxis(degrees,axis), Time.deltaTime );
	}

	#endregion


}
