using UnityEngine;
using System.Collections;

public enum Pickup{CureS,CureM,CureL,NAPAM,NUKE,RAZER,STEALTH,WAVE,YUDOU,ZENHOUKOU}

public class item : MonoBehaviour {

	public Pickup pickType=Pickup.CureS;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	void OnTriggerEnter(Collider other) {
//		if(other.gameObject.layer==LayerMask.NameToLayer("Ship")){
//			//Shipの時だけ動作するようにする
//			Destroy(this.gameObject);
//			AudioController.Play ("Powerup");
//		}
//	}
}
