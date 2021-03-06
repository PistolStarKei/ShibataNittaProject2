﻿using UnityEngine;
using System.Collections;
using PathologicalGames;

public class shot : SubweaponShot {
	public DanmakuColor shotCol;
	public override void Spawn(shipControl launcherShip,float spawnTime,Vector3 spawnPos,ShipOffset offset,string ID){
		this.launcherShip=launcherShip;
		this.ID=ID;
		hit=0;
		transform.position=launcherShip.transform.position+launcherShip.GetShotOffset(offset);
		weponType=Subweapon.NONE;
		this.spawnTime=spawnTime;
		this.spawnPos=spawnPos;
		ellapsedTime=0.0f;
		lastellapsedTime=0.0f;
		life=PSParams.GameParameters.shot_life[(int)shotCol];
		damage=PSParams.GameParameters.shot_damage[(int)shotCol];
		shotSpeed=PSParams.GameParameters.shot_speed[(int)shotCol];
	}

	public override void EffectAndDead(shipControl ship){
		
		if(GUIManager.Instance.IsWithinAudioDistance(ship.transform.position))AudioController.Play("Hit",ship.transform.position,ship.transform);

		ParticleManager.Instance.ShowExplosionSmallAt(ship.transform.position,Quaternion.identity,null);

		KillSelf();
	}

	public float shotSpeed=1.0f;
	public override void Move(){
		//transform.Translate(Vector3.forward * Time.deltaTime*shotSpeed);
		if(ellapsedTime>0.0f)transform.position= GetEllapsedPosition(spawnPos,transform.forward,ellapsedTime);
	}

	Vector3 GetEllapsedPosition(Vector3 spawnAt,Vector3 vector,float ellapsedTime){
		return spawnAt+vector*(ellapsedTime*shotSpeed);
	}

	public  override void OnCollideShip(shipControl ship){
		Debug.Log("shot OnCollideShip");
		base.OnCollideShip(ship);
	}

	public override  void OnCollideWall(){
		ParticleManager.Instance.ShowExplosionCollideAt(transform.position,Quaternion.identity,null);
		base.OnCollideWall();

	}


}
