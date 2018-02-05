using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MuzzleManagerの説明
/// </summary>
public class MuzzleManager : MonoBehaviour {

	#region  メンバ変数
	public ParticleSystem psF;
	public ParticleSystem psL;
	public ParticleSystem psR;
	public float scale;

	#endregion

	#region  初期化

	void Awake () {
		transform.localPosition=Vector3.zero;
		SetScale(scale);
	}
	#endregion

	#region  Public関数
	public void Emit(Vector3 pos){
		psF.transform.position=pos;
		EmitForwad(1);
	}
	public void Emit(Vector3 pos,Vector3 pos2){
		psL.transform.position=pos;
		psR.transform.position=pos2;
		EmitLeft(1);
		EmitRight(1);
	}

	public void Emit(Vector3 pos,Vector3 pos2,Vector3 pos3){
		psL.transform.position=pos;
		psR.transform.position=pos2;
		psF.transform.position=pos3;
		EmitForwad(1);
		EmitLeft(1);
		EmitRight(1);
	}



	public void SetPosition(Vector3 forwad,Vector3 Left,Vector3 right){
		psF.gameObject.transform.localPosition=forwad;
		psL.gameObject.transform.localPosition=Left;
		psR.gameObject.transform.localPosition=right;
	}

	#endregion
	

	#region  メンバ関数
	void EmitForwad(int scale){
		
		psF.Emit(scale);
	}
	void EmitLeft(int scale){
		psL.Emit(scale);
	}
	void EmitRight(int scale){
		psR.Emit(scale);
	}

	void SetScale(float scale){
		psF.startSize=scale;
		psL.startSize=scale;
		psR.startSize=scale;
	}


	#endregion
}