using UnityEngine;
using System.Collections;
using PathologicalGames;

public enum Pickup{CureS,CureM,CureL,NAPAM,NUKE,RAZER,STEALTH,WAVE,YUDOU,ZENHOUKOU}

public class item : MonoBehaviour {

	public Pickup pickType=Pickup.CureS;

	void Start(){
		if(gameObject.layer!=LayerMask.NameToLayer("PickUp")){
			gameObject.layer=LayerMask.NameToLayer("PickUp");
		}
	}



	public void KillSelf(){
		PoolManager.Pools["Weapons"].Despawn(gameObject.transform);
	}


	void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == LayerMask.NameToLayer("Ship")){

			shipControl ship=other.gameObject.GetComponent<shipControl>();
			if(!ship || !ship.isOwnersShip()){
				return;
			}

			if(pickType==Pickup.CureS || pickType==Pickup.CureM ||pickType==Pickup.CureL){
				ship.OnPickUp_Cure(pickType);
				KillSelf();
			}else{
				if(GUIManager.Instance.ISSubweponHolderHasSpace()){
					ship.OnPickUp_Subweapon(pickType);
					KillSelf();
				}else{
					return;
				}

			}

		}else{
			KillSelf();
		}
	}
}
