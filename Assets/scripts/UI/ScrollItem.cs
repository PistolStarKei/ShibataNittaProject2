using UnityEngine;
using System.Collections;

public class ScrollItem : MonoBehaviour {

	public ShipSelecter selecter;
	// Use this for initialization
	void OnClick () {
		selecter.OnClickItem(gameObject.name);
	}


	public UILabel itemTittle;
	public UISprite shipSp;
	public UISprite bgSp;

	public void SetState(bool isOn){
		if(isOn){
			if(bgSp.enabled)return;
			shipSp.alpha=selecter.enableAlpha;
			bgSp.enabled=true;
		}else{
			if(!bgSp.enabled)return;
			shipSp.alpha=selecter.disbleAlpha;
			bgSp.enabled=false;
		}
	}


}
