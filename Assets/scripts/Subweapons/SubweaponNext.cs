using UnityEngine;
using System.Collections;

public class SubweaponNext : MonoBehaviour {

	public UISprite spItem;

	public void SetToEmpty(){
		spItem.spriteName="bg_box";
	}

	public void SetItem(string name){

		Debug.Log("SetItem "+name);
		spItem.enabled=true;
		spItem.spriteName=name;
	}
}
