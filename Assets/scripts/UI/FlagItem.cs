using UnityEngine;
using System.Collections;

[RequireComponent (typeof (UISprite))]
public class FlagItem : MonoBehaviour {

	UISprite sp;
	// Use this for initialization
	void Awake () {
		sp=gameObject.GetComponent<UISprite>();
	}

	public void Init(string str,Callback_OnClicked onClicked,bool initialState){
		if(!sp)sp=gameObject.GetComponent<UISprite>();

		this.onClicked=onClicked;gameObject.name=str;sp.spriteName=str;

		SetState(initialState);
	}

	void OnClick(){
		if(onClicked!=null)onClicked(this);
	}
	public delegate void Callback_OnClicked(FlagItem item);
	public event Callback_OnClicked onClicked;

	public void SetState(bool isSelected){
		if(isSelected){
			sp.alpha=1.0f;
		}else{
			sp.alpha=0.3f;
		}
	}



}
