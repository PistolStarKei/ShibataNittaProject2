using UnityEngine;
using System.Collections;
using PathologicalGames;

[RequireComponent (typeof (SphereCollider))]
[RequireComponent (typeof (Rigidbody))]
public class SubweaponEffecter : MonoBehaviour {

	public float damage = 100.0f;
	public shipControl launcherShip;
	public Subweapon weponType=Subweapon.NONE;

	void OnEnable()
	{
		StartCoroutine (ParticleWorking ());
	}

	public ParticleSystem particle;
	IEnumerator ParticleWorking()
	{
		yield return new WaitWhile (() => particle.IsAlive (true));

		KillSelf();
	}

	public virtual void Spawn(shipControl launcherShip){
		this.launcherShip=launcherShip;
	}

	public virtual void OnShipEnter(shipControl ship){
		if(ship){
			if(!launcherShip){
				return;
			}


			if(ship!=launcherShip){
				//発射した機体以外の場合
				if(!ship.isDead)ship.OnHit(launcherShip,Subweapon.NONE,damage,"");
			}else{
				//自機であった場合
			}
		}
	}

	void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == LayerMask.NameToLayer("Ship")){
			shipControl ship=other.gameObject.GetComponent<shipControl>();
			OnShipEnter(ship);


		}
	}



	public void KillSelf(){
		PoolManager.Pools["Weapons"].Despawn(gameObject.transform);
	}



}
