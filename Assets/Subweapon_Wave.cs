using UnityEngine;
using System.Collections;

public class Subweapon_Wave : SubweaponShot {

	public float shotSpeed=1.0f;
	public override void Move(){
		if(ellapsedTime>0.0f)transform.position= GetEllapsedPosition(spawnPos,transform.forward,ellapsedTime);
	}

	Vector3 GetEllapsedPosition(Vector3 spawnAt,Vector3 vector,float ellapsedTime){
		return spawnAt+vector*(ellapsedTime*shotSpeed);
	}

	public override void EffectAndDead(Vector3 effectPosition){
		ParticleManager.Instance.ShowExplosionSmallAt(effectPosition,Quaternion.identity,this.transform);
		KillSelf();
	}

	public  override void OnCollideShip(shipControl ship){
		base.OnCollideShip(ship);
	}

	public override  void OnCollideWall(){
		base.OnCollideWall();

	}


}
