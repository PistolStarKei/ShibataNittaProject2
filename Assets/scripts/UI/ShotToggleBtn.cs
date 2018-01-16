using UnityEngine;
using System.Collections;

public class ShotToggleBtn : MonoBehaviour {

	public bool toggleVal=false;

	void Start(){
		SetToggle(toggleVal);
	}

	public void SetToggle(bool isOn){
		this.toggleVal=isOn;
		sp.enabled=isOn;
	}

	public UISprite sp;

	public void OnClickToggle(){
		
		if(GUIManager.Instance.OnShootBtnToggle(!this.toggleVal)){
			this.toggleVal=!this.toggleVal;
			SetToggle(this.toggleVal);
		}
	}

}
