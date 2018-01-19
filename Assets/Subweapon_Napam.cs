using UnityEngine;
using System.Collections;

public class Subweapon_Napam : SubweaponShot {

	public float shotSpeed=1.0f;
	public override void Move(){
		//transform.Translate(Vector3.forward * Time.deltaTime*shotSpeed);
		transform.position= GetEllapsedPosition(spawnPos,transform.forward,ellapsedTime);
	}

	Vector3 GetEllapsedPosition(Vector3 spawnAt,Vector3 vector,float ellapsedTime){
		return spawnAt+vector*(ellapsedTime*shotSpeed);
	}

	public override void KillTimer(){
		//爆破オブジェクトをスポーンして居なくなる
		if(launcherShip && !launcherShip.isDead)PickupAndWeaponManager.Instance.SpawnSubweapon_NapamEffecter(launcherShip,this.transform.position,Quaternion.identity,null);
		KillSelf();
	}

	public  override void OnCollideShip(shipControl ship,Vector3 hitpoint){
		if(ship){
			if(!launcherShip || launcherShip.isDead){
				KillSelf();
				return;
			}
			if(ship!=launcherShip){
				//発射した機体以外の場合
				KillTimer();


			}else{
				//自機であった場合
			}
		}
	}

	public override  void OnCollideWall(){
		KillTimer();

	}
}
