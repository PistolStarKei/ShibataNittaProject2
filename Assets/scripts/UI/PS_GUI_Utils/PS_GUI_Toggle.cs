using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PS_GUI_Toggle : MonoBehaviour {

	public bool isOn=false;
	public List<EventDelegate> onClick = new List<EventDelegate>();

	public void SetVisible(bool isOn){
		gameObject.SetActive(isOn);
	}

	public void SetState(bool isOn){
		if(isOn){
			StateToOn();
		}else{
			StateToOff();
		}
	}

	// Use this for initialization
	void OnClick () {
		EventDelegate.Execute(onClick);

	}

	void StateToOn(){
		sp.color=spOn;
		lb.color=lbOn;
		isOn=true;
	}
	void StateToOff(){
		sp.color=spOff;
		lb.color=lbOff;
		isOn=false;
	}

	public UISprite sp;
	public Color spOn;
	public Color spOff;

	public UILabel lb;
	public Color lbOn;
	public Color lbOff;

}
