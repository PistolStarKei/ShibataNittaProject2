﻿using UnityEngine;
using System.Collections;
using PathologicalGames;
public enum shotType{WHITE,BLUE,YELLOW,GREEN,RED}
public class shot : MonoBehaviour {

	[HideInInspector]
	public float damage = 100.0f;
	[HideInInspector]
	public float life = 60.0f;
	[HideInInspector]
	public float shotSpeed=1.0f;

	public shipControl launcherShip;
	public shotType type;

	float spawnTime=0.0f;
	Vector3 spawnPos;
	float ellapsedTime=0.0f;
	public void Spawn(shipControl launcherShip,float spawnTime,Vector3 spawnPos){
		this.launcherShip=launcherShip;

		this.spawnTime=spawnTime;
		this.spawnPos=spawnPos;
		ellapsedTime=0.0f;

		life=PSParams.GameParameters.shot_life[(int)type];
		damage=PSParams.GameParameters.shot_damage[(int)type];
		shotSpeed=PSParams.GameParameters.shot_speed[(int)type];

	}

	void Update(){
		ellapsedTime=GetEllapsedTime();
		if(ellapsedTime>life){
			KillSelf();
			return;
		}

		Move();
	}

	void Move(){
		transform.position=spawnPos+(transform.forward*(ellapsedTime*shotSpeed));
	}

	float GetEllapsedTime(){
		if(!PSPhoton.GameManager.instance.isNetworkMode){
			return Time.realtimeSinceStartup-spawnTime;
		}else{
			return PSGameUtils.GameUtils.ConvertToFloat((float)(PhotonNetwork.time-spawnTime));
		}
	}


	void KillSelf(){
		PoolManager.Pools["Weapons"].Despawn(gameObject.transform);
	}

	void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == LayerMask.NameToLayer("Ship")){
			shipControl ship=other.gameObject.GetComponent<shipControl>();

			if(ship!=launcherShip){
				//発射した機体以外の場合
				Vector3 hitpoint=other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

				if(ship){
					ship.OnHit(Subweapon.NONE,damage,hitpoint,launcherShip);
					KillSelf();
				}
			}else{
				//自機であった場合
			}
		}

		if(other.gameObject.layer == LayerMask.NameToLayer("enemy")){
			AudioController.Play ("Explosion");
			KillSelf();
		}

		if(other.gameObject.layer == LayerMask.NameToLayer("Wall")){
			KillSelf();
		}
	}


	/*void OnCollisionEnter(Collision collision) {

		Debug.Log("OnCollisionEnter"+ collision.gameObject.name);

		if(collision.gameObject.layer == LayerMask.NameToLayer("Ship")){
			shipControl ship=collision.gameObject.GetComponent<shipControl>();

			if(ship!=launcherShip){
				//発射した機体以外の場合
				Vector3 hitpoint=collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

				if(ship){
					ship.OnHit(damage,hitpoint);
					KillSelf();
				}
			}else{
				//自機であった場合
			}
		}

		if(collision.gameObject.layer == LayerMask.NameToLayer("enemy")){
			AudioController.Play ("Explosion");
			KillSelf();
		}

		if(collision.gameObject.layer == LayerMask.NameToLayer("Wall")){
			KillSelf();
		}

	}*/
}
