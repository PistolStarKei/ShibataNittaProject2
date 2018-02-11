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
	#endregion

	#region  初期化

	void Awake () {
		this.gameObject.name="Muzzle";
		
		transform.localPosition=Vector3.zero;


		GameObject go=Resources.Load("batibati")as GameObject;
		GameObject go2=Instantiate(go,this.transform) as GameObject;
		psF=go2.GetComponent<ParticleSystem>();
		go2.transform.parent=this.transform;

		go2=Instantiate(go,this.transform) as GameObject;
		psL=go2.GetComponent<ParticleSystem>();
		go2.transform.parent=this.transform;

		go2=Instantiate(go,this.transform) as GameObject;
		psR=go2.GetComponent<ParticleSystem>();
		go2.transform.parent=this.transform;

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




	#endregion
}