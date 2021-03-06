﻿using UnityEngine;
using System.Collections;

public class Subweapon_Yudoudan : SubweaponShot {

	public float shotSpeed=1.0f;

	public Transform target;
	public float damping = 1.0f;
	public float chasingSpeed = 15.0f;

	Quaternion rotation;
	public float stillTime = 2.0f;

	public void Spawn(shipControl launcherShip,float spawnTime,Vector3 spawnPos,ShipOffset offset,string ID,int shipID){

		Debug.Log("誘導弾　"+ID);

		if(GUIManager.Instance.IsWithinAudioDistance(spawnPos))AudioController.Play("Missile",spawnPos,null);

		base.Spawn(launcherShip,spawnTime,spawnPos,offset,ID);
		shipControl ship=PSPhoton.GameManager.instance.GetShipById(shipID);
		if(ship)target=ship.transform;

	}
			
	public override void Move(){
		
		if(ellapsedTime<stillTime)return;
			
		if(target){
			
			rotation = Quaternion.LookRotation(target.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (ellapsedTime-stillTime) * damping);
			transform.Translate(Vector3.forward * Time.deltaTime * chasingSpeed);

		}else{
			transform.Translate(Vector3.forward * Time.deltaTime*shotSpeed);

		}

	}

	public override void KillTimer(){
		//ここでエフェクトをだす
		if(GUIManager.Instance.IsWithinAudioDistance(transform.position))AudioController.Play("Yudou",transform.position,null);

		ParticleManager.Instance.ShowExplosionSmallAt(transform.position,Quaternion.identity,null);
		KillSelf();
	}

	public override void EffectAndDead(shipControl ship){
		if(GUIManager.Instance.IsWithinAudioDistance(transform.position))AudioController.Play("Yudou",transform.position,null);
		if(ship!=null)ParticleManager.Instance.ShowExplosionSmallAt(ship.transform.position,Quaternion.identity,null);
		KillSelf();
	}

	public  override void OnCollideShip(shipControl ship){
		if(ship){
			if(!launcherShip){
				return;
			}


			if(ship!=launcherShip){
				//発射した機体以外の場合
				if(!ship.isDead){
					launcherShip.OnHit_Hitter(ship,Subweapon.YUDOU,damage,ID);
					ship.OnHit(launcherShip,Subweapon.YUDOU,damage,ID);
				}else{
					//死んだ機体の場合
				}
			}else{
				//自機であった場合
			}
		}

	}

	public override  void OnCollideWall(){
		if(GUIManager.Instance.IsWithinAudioDistance(transform.position))AudioController.Play("Yudou",transform.position,null);
		ParticleManager.Instance.ShowExplosionSmallAt(transform.position,Quaternion.identity,null);
		KillSelf();
	}
}
