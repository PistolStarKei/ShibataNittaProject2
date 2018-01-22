using UnityEngine;
using System.Collections;

public class Subweapon_Yudoudan : SubweaponShot {

	public float shotSpeed=1.0f;

	public Transform target;
	public float damping = 1.0f;
	public float chasingSpeed = 15.0f;

	Quaternion rotation;
	public float serchingDistance=5.0f;


	public override void Spawn(shipControl launcherShip,float spawnTime,Vector3 spawnPos,ShipOffset offset,string ID){
		base.Spawn(launcherShip,spawnTime,spawnPos,offset,ID);
		SearchTarget();
	}

	void SearchTarget(){
		//近い敵から候補を見つける
		GameObject[] go=GameObject.FindGameObjectsWithTag("Player");
		target=null;
		if(go.Length>0){
			GameObject nearest=go[0];
			float dist=GetDistanceFromMe(go[0].transform.position);
			if(go.Length==1){
				if(dist<serchingDistance){
					target=nearest.transform;
				}else{
					target=null;
				}
			}else{
				float cul=0.0f;

				for(int i=1;i<go.Length;i++){
							cul=GetDistanceFromMe(go[i].transform.position);
							if(dist>cul){
								dist=cul;
								nearest=go[i];
							}


				}
					if(dist<serchingDistance){
						target=nearest.transform;
					}else{
						target=null;
					}
					
			}

		}

	}

	float GetDistanceFromMe(Vector3 pos){
		return Vector3.Distance(transform.position,pos);
	}
	public override void Move(){
		

		if(target){
			transform.Translate(Vector3.forward * Time.deltaTime * chasingSpeed);
			rotation = Quaternion.LookRotation(target.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
		}else{
			transform.Translate(Vector3.forward * Time.deltaTime*shotSpeed);

		}

	}

	public override void KillTimer(){
		//ここでエフェクトをだす
		ParticleManager.Instance.ShowExplosionSmallAt(transform.position,Quaternion.identity,null);
		KillSelf();
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
					//ship.OnHit(launcherShip,Subweapon.NONE,damage,ID);
				}else{
					//死んだ機体の場合
				}
			}else{
				//自機であった場合
			}
		}

	}

	public override  void OnCollideWall(){
		ParticleManager.Instance.ShowExplosionSmallAt(transform.position,Quaternion.identity,null);
		KillSelf();
	}
}
