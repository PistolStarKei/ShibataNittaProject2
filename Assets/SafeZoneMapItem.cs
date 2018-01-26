using UnityEngine;
using System.Collections;


public enum ZoneState{None,Pending,Fixed}
[RequireComponent (typeof (UISprite))]
public class SafeZoneMapItem : MonoBehaviour {

	UISprite sp;
	public ZoneState state;
	public SafeZoneMap mapParent;
	// Use this for initialization
	void Awake() {
		sp=gameObject.GetComponent<UISprite>();
		ta=gameObject.GetComponent<TweenColor>();

	}
	TweenColor ta;
	void Start(){
		SetStateTo(ZoneState.None);
		ta.from=mapParent.colDefault;
		ta.to=mapParent.colDanger;
		ta.duration=mapParent.tweenDulation;
	}
	public void SetStateTo(ZoneState state){
		this.state=state;
		switch(state){
			case ZoneState.None:
				ta.enabled=false;
				sp.color=mapParent.colDefault;
				break;
			case ZoneState.Pending:
				ta.enabled=true;
				sp.color=mapParent.colDanger;
				break;
			case ZoneState.Fixed:
				ta.enabled=false;
				sp.color=mapParent.colDanger;
				break;
		}
	}

}
