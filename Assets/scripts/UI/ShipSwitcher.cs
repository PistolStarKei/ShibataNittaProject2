using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSwitcher : PS_SingletonBehaviour<ShipSwitcher> {

	public List<GameObject> ships;
	public GUI_ShipRotater currentShip;
	// Use this for initialization
	void Awake () {
		foreach (Transform child in transform){
			ships.Add(child.gameObject);
		}
	}
	
	public void Set(int index){
		ClearAll();
		ships[index].SetActive(true);
		currentShip=ships[index].GetComponent<GUI_ShipRotater>();
		currentShip.SetToDefault();

	}

	void ClearAll(){
		foreach (GameObject go in ships){
			go.SetActive(false);
		}
	}


}
