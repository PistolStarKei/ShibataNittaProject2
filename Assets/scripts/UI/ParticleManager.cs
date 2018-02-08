using UnityEngine;
using System.Collections;
using PathologicalGames;

public class ParticleManager : PS_SingletonBehaviour<ParticleManager> {


	public GameObject explosionBig;
	public GameObject explosionSmall;
	public GameObject explosionCollide;
	public GameObject cureS;
	public GameObject cureM;
	public GameObject cureL;


	public void ShowCureSAt(Vector3 position,Quaternion qt,Transform parent){
		SpawnParticle(cureS,position,qt,parent);
	}
	public void ShowCureMAt(Vector3 position,Quaternion qt,Transform parent){
		SpawnParticle(cureM,position,qt,parent);
	}
	public void ShowCureLAt(Vector3 position,Quaternion qt,Transform parent){
		SpawnParticle(cureL,position,qt,parent);
	}

	Transform current;
	public string poolName="Particles";
	public void ShowExplosionBigAt(Vector3 position,Quaternion qt,Transform parent){
		SpawnParticle(explosionBig,position,qt,parent);
	}

	public void ShowExplosionSmallAt(Vector3 position,Quaternion qt,Transform parent){
		SpawnParticle(explosionSmall,position,qt,parent);
	}
	public void ShowExplosionCollideAt(Vector3 position,Quaternion qt,Transform parent){
		SpawnParticle(explosionCollide,position,qt,parent);
	}

	void SpawnParticle(GameObject prefab,Vector3 position,Quaternion qt,Transform parent){
		

		if(parent!=null){
			current=PoolManager.Pools[poolName].Spawn
				(
					prefab, 
					position, 
					qt,
					parent
				);
		}else{
			current=PoolManager.Pools[poolName].Spawn
				(
					prefab, 
					position, 
					qt
				);
		}
	}
}
