using UnityEngine;
using System.Collections;

public class Subweapon_Nuke : SubweaponShot {

	public float shotSpeed=1.0f;

	public override void Spawn(shipControl launcherShip,float spawnTime,Vector3 spawnPos,ShipOffset offset,string ID){

		if(GUIManager.Instance.IsWithinAudioDistance(spawnPos))AudioController.Play("Missile",spawnPos,null);
		base.Spawn(launcherShip,spawnTime,spawnPos,offset,ID);
	}

	public override void Move(){
		//transform.Translate(Vector3.forward * Time.deltaTime*shotSpeed);
		if(ellapsedTime>0.0f)transform.position= GetEllapsedPosition(spawnPos,transform.forward,ellapsedTime);
	}

	Vector3 GetEllapsedPosition(Vector3 spawnAt,Vector3 vector,float ellapsedTime){
		return spawnAt+vector*(ellapsedTime*shotSpeed);
	}

	public override void KillTimer(){
		//爆破オブジェクトをスポーンして居なくなる
		launcherShip.ShotNuke_Effect(transform.position);
		launcherShip.RemoveWeaponHolder(this);
		KillSelf();
	}

	public override void EffectAndDead(shipControl ship){
		KillTimer();
	}

	public  override void OnCollideShip(shipControl ship){
		return;
	}

	public override  void OnCollideWall(){
		KillTimer();

	}
}
