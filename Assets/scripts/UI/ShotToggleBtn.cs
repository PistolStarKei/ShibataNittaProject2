using UnityEngine;
using System.Collections;

public class ShotToggleBtn : MonoBehaviour {

	public bool toggleVal=false;

	void Start(){
		SetToggle(toggleVal);
	}

	public Transform btnTrans;
	public Vector3 transOn;
	public Vector3 transOff;
	public Vector3 transLbOn;
	public Vector3 transLbOff;
	public Color bgColorOn;
	public Color bgColorOff;

	public UILabel lb;
	public Color lbColorOn;
	public Color lbColorOff;

	public void SetToggle(bool isOn){
		this.toggleVal=isOn;
		if(isOn){
			btnTrans.localPosition=transOn;
			lb.transform.localPosition=transLbOn;
			sp.color=bgColorOn;
			lb.text="ON";
			lb.color=lbColorOn;
			lb.effectColor=lb.color;
		}else{
			btnTrans.localPosition=transOff;
			lb.transform.localPosition=transLbOff;
			sp.color=bgColorOff;
			lb.text="OFF";
			lb.color=lbColorOff;
			lb.effectColor=lb.color;
		}


	}

	public UISprite sp;

	public void OnClickToggle(){
		
		if(GUIManager.Instance.OnShootBtnToggle(!this.toggleVal)){
			this.toggleVal=!this.toggleVal;
			SetToggle(this.toggleVal);
		}
	}

}
