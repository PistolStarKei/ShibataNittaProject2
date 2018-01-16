﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapDetecterTrigger : MonoBehaviour {

	public List<shipControl> ships=new List<shipControl>();

	public shipControl GetNearestShip(Vector3 pos,float maxDistance){
		if(ships.Count<=0)return null;
		shipControl nearest=ships[0];
		float dist=GetDistanceFromMe(nearest.transform.position,pos);
		if(ships.Count==1){
			if(dist>maxDistance){
				nearest=null;
			}
		}else{
			float cul=0.0f;
			for(int i=1;i<ships.Count;i++){
				cul=GetDistanceFromMe(ships[i].transform.position,pos);
				if(dist>cul){
					dist=cul;
					nearest=ships[i];
				}
			}

			if(dist>maxDistance){
				nearest=null;
			}

		}

		return nearest;

	}
	float GetDistanceFromMe(Vector3 pos1,Vector3 pos2){
		return Vector3.Distance(pos1,pos2);
	}

	public bool isActive=true;

	public float radius = 0f;
	[Tooltip("もしfalseなら　インスペクタの値を使う　トリガーよりも大きな値は使えない　小さい値は、画面外の敵の描画に")]
	public bool userColliderRadius=true;
	public GameObject mapRestrict;

	void Start(){
		
		radius=mapRestrict.transform.lossyScale.x;
		Destroy(mapRestrict);
		//トリガーよりも大きな値は使えない
		radius=radius>((CapsuleCollider) GetComponent<Collider>()).radius? ((CapsuleCollider) GetComponent<Collider>()).radius:radius;
	}


	public Transform playerTrans;

	void LateUpdate(){
		if(playerTrans){
			transform.position=playerTrans.position;
			transform.rotation=Quaternion.Euler(0.0f,playerTrans.eulerAngles.y,0.0f);
		}
	}

	shipControl ship;
	void OnTriggerEnter(Collider other) {
		//自分は入れない
		if(other.transform==playerTrans)return;
			
		//Debug.Log("OnTriggerEnter "+other.name);
		ship=other.GetComponent<shipControl>();
		if(ship)ships.Add(ship);
	}

	void OnTriggerExit(Collider other)
	{
		//自分は入れない
		if(other.transform==playerTrans)return;
	
		//Debug.Log("OnTriggerExit "+other.name);
		ship=other.GetComponent<shipControl>();
		if(ship)ships.Remove(ship);
	}


	Vector3 newPos1;
	public Vector2 GetRelativePosition(Vector3 pos){
		newPos1 = transform.InverseTransformPoint(pos);

		if(newPos1.magnitude > radius)
		{
			Vector3 normPos1 = newPos1.normalized;
			newPos1 = normPos1 * radius;
		}
		float x = newPos1.x / radius;
		float z = newPos1.z / radius;

		//Debug.Log(""+delta.ToString());
		return new Vector2(x > 1f ? 1f : x, z > 1f ? 1f : z);

	}

}