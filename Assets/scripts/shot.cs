using UnityEngine;
using System.Collections;
using PathologicalGames;
public class shot : MonoBehaviour {

	public float damage = 100.0f;
	public float life = 60.0f;
	public float shotSpeed=1.0f;
	public shipControl launcherShip;

	public void Spawn(shipControl launcherShip){
		this.launcherShip=launcherShip;

		Invoke("KillSelf",life);
	}

	void Update(){
		Move();
	}

	void Move(){
		transform.Translate(Vector3.forward * Time.deltaTime*shotSpeed);
	}

	void KillSelf(){
		CancelInvoke("KillSelf");
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
