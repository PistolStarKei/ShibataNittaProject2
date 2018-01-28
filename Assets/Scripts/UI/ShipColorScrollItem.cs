using UnityEngine;
using System.Collections;

public class ShipColorScrollItem : MonoBehaviour {

	// Use this for initialization

	public ShipColorLists shipColorLists;
	void OnClick () {
		shipColorLists.OnClickItem(this);
	}

	public void SetSprite(string name){
		shipSp.spriteName=name;
	}

	public UISprite shipSp;
	public UISprite bgSp;

	public void SetState(bool isOn){
		if(isOn){
			if(bgSp.enabled)return;
			bgSp.enabled=true;
		}else{
			if(!bgSp.enabled)return;
			bgSp.enabled=false;
		}
	}

	public void KillSelf(){
		DestroyImmediate(gameObject);
	}
}
