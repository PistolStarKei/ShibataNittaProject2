using UnityEngine;
using System.Collections;
using PathologicalGames;

public enum Subweapon{NAPAM,NUKE,RAZER,STEALTH,WAVE,YUDOU,ZENHOUKOU,NONE}

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class SubweaponShot : MonoBehaviour {

	public float damage = 100.0f;
	public float life = 60;
	public int hit=0;
	void Start(){
		if(gameObject.layer!=LayerMask.NameToLayer("Shot"))gameObject.layer=LayerMask.NameToLayer("Shot");
	}

	public shipControl launcherShip;
	public Subweapon weponType=Subweapon.NONE;

	internal float spawnTime=0.0f;
	internal Vector3 spawnPos;
	internal float ellapsedTime=0.0f;
	public string ID;
	public virtual void Spawn(shipControl launcherShip,float spawnTime,Vector3 spawnPos,ShipOffset offset,string ID){
		
		this.launcherShip=launcherShip;
		ellapsedTime=0.0f;
		this.ID=ID;
		lastellapsedTime=0.0f;
		this.spawnPos=spawnPos;
		this.spawnTime=spawnTime;
		transform.position=launcherShip.transform.position+launcherShip.GetShotOffset(offset);

		hit=0;
		if(weponType!=Subweapon.NONE)damage=PSParams.GameParameters.sw_damage[(int)weponType];
		if(weponType!=Subweapon.NONE)life=PSParams.GameParameters.sw_life[(int)weponType];
	}

	public virtual void Move(){
	}

	public virtual void EffectAndDead(shipControl ship){

	}

	public virtual void KillTimer(){
		KillSelf();
	}

	public virtual void OnCollideShip(shipControl ship){
		
		if(ship){
			
			if(!launcherShip){
				return;
			}

			if(ship!=launcherShip){
				//発射した機体以外の場合
				if(!ship.isDead){
					hit++;
					Debug.Log("subweapon shot OnCollideShip HIt"+hit);
					launcherShip.OnHit_Hitter(ship,Subweapon.NONE,damage,ID);

					ship.OnHit(launcherShip,Subweapon.NONE,damage,ID);
				}

			}else{
				//自機であった場合
			}
		}
	}

	public virtual void OnCollideWall(){
		KillSelf();
	}

	public bool needPrediction=true;
	internal float lastellapsedTime=0.0f;
	void Update(){


		ellapsedTime=GetEllapsedTime();
		if(ellapsedTime==lastellapsedTime){
			if(needPrediction)ellapsedTime=ellapsedTime+Time.deltaTime;
		}
		if(ellapsedTime>life){
			KillTimer();
			return;
		}
		lastellapsedTime=ellapsedTime;
		Move();
	}

	float GetEllapsedTime(){
		if(!PSPhoton.GameManager.instance.isNetworkMode){
			return Time.realtimeSinceStartup-spawnTime;
		}else{
			return PSGameUtils.GameUtils.ConvertToFloat((float)(PhotonNetwork.time-spawnTime));
		}
	}

	public void KillSelf(){
		launcherShip.RemoveWeaponHolder(this);
		PoolManager.Pools["Weapons"].Despawn(gameObject.transform);
	}

	void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == LayerMask.NameToLayer("Ship")){
			shipControl ship=other.gameObject.GetComponent<shipControl>();

			if(ship)OnCollideShip(ship);


		}

		if(other.gameObject.layer == LayerMask.NameToLayer("Wall")){
			OnCollideWall();
		}
	}


}
