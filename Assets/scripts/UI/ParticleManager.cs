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

	public GameObject getItem;
	string[] fittingName=new string[7]{"PU_Napam","PU_Nuke","PU_Razer","PU_Stealth","PU_Wave","PU_Yudou","PU_Zenhoukou"};
	public Transform getItemParent;

	public void ShowGetItemEffect(Subweapon weapon){
		Transform trans=SpawnParticle(getItem,Vector3.zero,Quaternion.identity,getItemParent);
		trans.localPosition=Vector3.zero;
		trans.localScale=Vector3.one;
		trans.localRotation=Quaternion.identity;

		if(trans!=null){
			GetItemEffect gi=trans.gameObject.GetComponent<GetItemEffect>();
			if(gi!=null){
				gi.OnInit(weapon,fittingName[(int)weapon]);
			}else{
				Debug.LogError("gi is null!!");
			}
		}else{
			Debug.LogError("trans is null!!");
		}

	}
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
		SpawnParticle(explosionBig,position,Quaternion.Euler(new Vector3(-90f,0f,0f)),parent);
	}

	public void ShowExplosionSmallAt(Vector3 position,Quaternion qt,Transform parent){
		SpawnParticle(explosionSmall,position,Quaternion.Euler(new Vector3(-90f,0f,0f)),parent);
	}
	public void ShowExplosionCollideAt(Vector3 position,Quaternion qt,Transform parent){
		SpawnParticle(explosionCollide,position,qt,parent);
	}

	Transform SpawnParticle(GameObject prefab,Vector3 position,Quaternion qt,Transform parent){
		

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

		return current;
	}
}
