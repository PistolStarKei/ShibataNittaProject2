using UnityEngine;
using System.Collections;

public class ShotToggleBtn : MonoBehaviour {

	public bool toggleVal=false;

	void Start(){
		SetToggle(toggleVal);
	}

	public Color bgColorOn;
	public Color bgColorOff;
	public Color bgColorOnWaku;
	public Color bgColorOffWaku;

	public UILabel lb;

	public void SetToggle(bool isOn){
		this.toggleVal=isOn;
		if(isOn){
			sp.color=bgColorOn;
			spWake.color=bgColorOnWaku;
			lb.text="ON";
			lb.effectColor=lb.color;
		}else{
			sp.color=bgColorOff;
			lb.text="OFF";
			spWake.color=bgColorOffWaku;
			lb.effectColor=lb.color;
		}


	}

	public UISprite sp;
	public UISprite spWake;

	public void OnClickToggle(){
		
		if(GUIManager.Instance.OnShootBtnToggle(!this.toggleVal)){
			this.toggleVal=!this.toggleVal;
			SetToggle(this.toggleVal);
		}
	}

}
