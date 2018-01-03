using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSwitcher : MonoBehaviour {

	public List<GameObject> ships;

	// Use this for initialization
	void Awake () {
		foreach (Transform child in transform){
			ships.Add(child.gameObject);
		}
	}
	
	public void Set(int index){
		ClearAll();
		ships[index].SetActive(true);
		ships[index].GetComponent<GUI_ShipRotater>().SetToDefault();

	}

	void ClearAll(){
		foreach (GameObject go in ships){
			go.SetActive(false);
		}
	}


}
