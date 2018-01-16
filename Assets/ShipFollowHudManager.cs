using UnityEngine;
using System.Collections;
using PSGUI;

public class ShipFollowHudManager : MonoBehaviour {

	public Transform holder;
	public GameObject hudObjects;

	public void SetFollowhipHud(shipControl ship){
		AddNewHUD().SetTarget(ship);
	}

	ShipFollowHUD AddNewHUD(){
		GameObject go=Instantiate(hudObjects,Vector3.zero,Quaternion.identity) as GameObject;
		go.transform.parent=holder;
		go.transform.localScale=Vector3.one;
		go.transform.localPosition=Vector3.zero;
		go.transform.localRotation=Quaternion.Euler(Vector3.zero);

		ShipFollowHUD list=go.GetComponent<ShipFollowHUD>();
		return list? list: null;

	}


}
