using UnityEngine;
using System.Collections;
using PathologicalGames;

public enum Subweapon{NAPAM,NUKE,RAZER,STEALTH,WAVE,YUDOU,ZENHOUKOU,NONE}

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class SubweaponShot : MonoBehaviour {

	void Start(){
		if(gameObject.layer!=LayerMask.NameToLayer("Shot"))gameObject.layer=LayerMask.NameToLayer("Shot");
	}
	public float damage = 100.0f;
	public int life = 60;
	public shipControl launcherShip;
	public Subweapon weponType=Subweapon.NONE;

	public virtual void Spawn(shipControl launcherShip){
		this.launcherShip=launcherShip;

		if(life>0.0f)Invoke("KillTimer",life);
	}
	public virtual void Move(){
	}

	public virtual void KillTimer(){
		KillSelf();
	}

	public virtual void OnCollideShip(shipControl ship,Vector3 hitpoint){
		if(ship){
			if(ship!=launcherShip){
				//発射した機体以外の場合
				ship.OnHit(weponType,damage,hitpoint);


			}else{
				//自機であった場合
			}
		}
		KillSelf();
	}

	public virtual void OnCollideWall(){
		KillSelf();
	}


	void Update(){
		Move();
	}



	public void KillSelf(){
		CancelInvoke("KillTimer");
		PoolManager.Pools["Weapons"].Despawn(gameObject.transform);
	}



	void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == LayerMask.NameToLayer("Ship")){
			shipControl ship=other.gameObject.GetComponent<shipControl>();

			Vector3 hitpoint=other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
			OnCollideShip(ship,hitpoint);


		}

		if(other.gameObject.layer == LayerMask.NameToLayer("Wall")){
			OnCollideWall();
		}
	}


}
