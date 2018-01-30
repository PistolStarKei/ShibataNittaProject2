using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSwitcher : PS_SingletonBehaviour<ShipSwitcher> {

	public List<GameObject> ships;
	public GUI_ShipRotater currentShip;

	public Vector3[] pos;
	public Vector3 currentTrans;

	public float speed=0.0f;

	// Use this for initialization
	void Awake () {
		foreach (Transform child in transform){
			ships.Add(child.gameObject);
		}
	}
	public AnimationCurve curve;
	public bool speedByCurve=false;
	float lerpVal=0.0f;
	float startTime=0.0f;
	void Update(){
		
		startTime+=Time.deltaTime;
		if(speedByCurve){
			lerpVal+=curve.Evaluate(startTime)*startTime;
		}else{
			lerpVal+=speed*startTime;
		}


		transform.position=Vector3.Lerp(transform.position,currentTrans,lerpVal);
	}
	
	public void Set(int index){
		//ClearAll();
		//ships[index].SetActive(true);
		currentShip=ships[index].GetComponent<GUI_ShipRotater>();
		currentShip.SetToDefault();
		currentTrans=pos[index];
		lerpVal=0.0f;
		startTime=0.0f;
	}

	void ClearAll(){
		foreach (GameObject go in ships){
			go.SetActive(false);
		}
	}


}
