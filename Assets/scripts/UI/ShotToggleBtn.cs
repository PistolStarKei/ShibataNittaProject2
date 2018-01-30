using UnityEngine;
using System.Collections;

public class ShotToggleBtn : MonoBehaviour {

	public bool toggleVal=false;

	void Start(){
		SetToggle(toggleVal);
	}

	public Color bgColorOn;
	public Color bgColorOff;

	public void SetToggle(bool isOn){
		this.toggleVal=isOn;
		if(isOn){
			sp.color=bgColorOn;
			spShot.color=sp.color;
		}else{
			sp.color=bgColorOff;
			spShot.color=sp.color;
		}


	}

	public UISprite sp;
	public UISprite spShot;

	public void OnClickToggle(){
		
		if(GUIManager.Instance.OnShootBtnToggle(!this.toggleVal)){
			this.toggleVal=!this.toggleVal;
			SetToggle(this.toggleVal);
		}
	}

}
