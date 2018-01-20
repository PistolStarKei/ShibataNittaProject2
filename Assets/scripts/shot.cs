using UnityEngine;
using System.Collections;
using PathologicalGames;
public enum shotType{WHITE,BLUE,YELLOW,GREEN,RED}
public class shot : MonoBehaviour {

	[HideInInspector]
	public float damage = 100.0f;
	[HideInInspector]
	public float life = 60.0f;
	[HideInInspector]
	public float shotSpeed=1.0f;

	public shipControl launcherShip;
	public shotType type;

	public float spawnTime=0.0f;
	Vector3 spawnPos;
	float ellapsedTime=0.0f;
	public void Spawn(shipControl launcherShip,float spawnTime,Vector3 spawnPos,ShipOffset offset){
		
		this.launcherShip=launcherShip;

		transform.position=launcherShip.transform.position+launcherShip.GetShotOffset(offset);

		this.spawnTime=spawnTime;
		this.spawnPos=spawnPos;
		ellapsedTime=0.0f;
		lastellapsedTime=0.0f;
		life=PSParams.GameParameters.shot_life[(int)type];
		damage=PSParams.GameParameters.shot_damage[(int)type];
		shotSpeed=PSParams.GameParameters.shot_speed[(int)type];

	}

	public bool needPrediction=true;
	float lastellapsedTime=0.0f;
	void Update(){
		ellapsedTime=GetEllapsedTime();
		if(ellapsedTime==lastellapsedTime){
			if(needPrediction)ellapsedTime=ellapsedTime+Time.deltaTime;
		}
		if(ellapsedTime>life){
			KillSelf();
			return;
		}
		lastellapsedTime=ellapsedTime;

	

		Move();
	}

	Vector3 GetEllapsedPosition(Vector3 spawnAt,Vector3 vector,float ellapsedTime){
		return spawnAt+vector*(ellapsedTime*shotSpeed);
	}

	void Move(){
		if(ellapsedTime>0.0f)transform.position= GetEllapsedPosition(spawnPos,transform.forward,ellapsedTime);
	}

	float GetEllapsedTime(){
		if(!PSPhoton.GameManager.instance.isNetworkMode){
			return Time.realtimeSinceStartup-spawnTime;
		}else{
			return PSGameUtils.GameUtils.ConvertToFloat((float)(PhotonNetwork.time-spawnTime));
		}
	}


	void KillSelf(){
		PoolManager.Pools["Weapons"].Despawn(gameObject.transform);
	}

	void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == LayerMask.NameToLayer("Ship")){
			
			if(!launcherShip){
				KillSelf();
				return;
			}

			shipControl ship=other.gameObject.GetComponent<shipControl>();

			if(ship!=launcherShip){
				//発射した機体以外の場合
				if(!ship.isDead){
					ship.OnHit(launcherShip,Subweapon.NONE,damage);
					KillSelf();
				}else{
					//死んだ機体の場合
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
