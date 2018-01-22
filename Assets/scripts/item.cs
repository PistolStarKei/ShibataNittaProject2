using UnityEngine;
using System.Collections;
using PathologicalGames;

public enum Pickup{CureS,CureM,CureL,NAPAM,NUKE,RAZER,STEALTH,WAVE,YUDOU,ZENHOUKOU}

[RequireComponent(typeof(PhotonView))]
public class item : Photon.MonoBehaviour, IPunObservable {

	public Pickup pickType=Pickup.CureS;


	void Start(){
		//念のため、レイヤを設定する
		if(gameObject.layer!=LayerMask.NameToLayer("PickUp")){
			gameObject.layer=LayerMask.NameToLayer("PickUp");
		}
	}



	public void KillSelf(){
		this.gameObject.SetActive(false);
	}


	void OnTriggerEnter(Collider other) {

		if(other.gameObject.layer == LayerMask.NameToLayer("Ship")){

			shipControl ship=other.gameObject.GetComponent<shipControl>();
			if(!ship || !ship.isOwnersShip()){
				return;
			}

			if(pickType==Pickup.CureS || pickType==Pickup.CureM ||pickType==Pickup.CureL){
				OnPickup();
			}else{
				if(GUIManager.Instance.ISSubweponHolderHasSpace()){
					OnPickup();
				}else{
					//すでにアイテムが一杯なので無視する
					return;
				}

			}




		}else{
			KillSelf();
		}
	}


	public bool SentPickup;

	public void OnPickup()
	{
		if (this.SentPickup)
		{
			// skip sending more pickups until the original pickup-RPC got back to this client
			return;
		}

		this.SentPickup = true;
		this.photonView.RPC("PunPickup", PhotonTargets.AllViaServer);
	}

	[PunRPC]
	public void PunPickup(PhotonMessageInfo msgInfo)
	{
		// when this client's RPC gets executed, this client no longer waits for a sent pickup and can try again
		if (msgInfo.sender.IsLocal) this.SentPickup = false;


		// In this solution, picked up items are disabled. They can't be picked up again this way, etc.
		// You could check "active" first, if you're not interested in failed pickup-attempts.
		if (!this.gameObject.GetActive())
		{
			// optional logging:
			Debug.Log("Ignored PU RPC, cause item is inactive. " + this.gameObject);
			return;     // makes this RPC being ignored
		}


		// msgInfo.sender.IsLocalならば、自分のRPCだと確認できる
		// 取得者のRPCなのでshipのOnPickUpを呼び出す
		if (msgInfo.sender.IsLocal != null)
		{
			
			if(pickType==Pickup.CureS || pickType==Pickup.CureM ||pickType==Pickup.CureL){
				PSPhoton.GameManager.instance.playerShip.OnPickUp_Cure(pickType);
			}else{
				PSPhoton.GameManager.instance.playerShip.OnPickUp_Subweapon(pickType);

			}

		}

		KillSelf();
	}



	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		// read the description in SecondsBeforeRespawn

		if (stream.isWriting)
		{
			stream.SendNext(this.gameObject.transform.position);
		}
		else
		{
			// this will directly apply the last received position for this PickupItem. No smoothing. Usually not needed though.
			Vector3 lastIncomingPos = (Vector3)stream.ReceiveNext();
			this.gameObject.transform.position = lastIncomingPos;
		}
	}
}
