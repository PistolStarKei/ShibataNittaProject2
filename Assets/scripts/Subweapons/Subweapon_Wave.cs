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

	public override void EffectAndDead(shipControl ship){
		ParticleManager.Instance.ShowExplosionSmallAt(ship.transform.position,Quaternion.identity,ship.transform);

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
					launcherShip.OnHit_Hitter(ship,weponType,damage,ID);
					ship.OnHit(launcherShip,weponType,damage,ID);
				}

			}else{
				//自機であった場合
			}
		}
	}

	public override  void OnCollideWall(){
		ParticleManager.Instance.ShowExplosionCollideAt(transform.position,Quaternion.identity,null);
		base.OnCollideWall();

	}


}
