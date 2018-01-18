using UnityEngine;
using System.Collections;

[RequireComponent (typeof (UILabel))]
public class PS_GUI_LabelAnimation : MonoBehaviour {

	UILabel label;
	public TweenScale ts;

	int num=0;
	public void Test(){
		SetNum(num);
		num++;
	}
	// Use this for initialization
	void Awake () {
		label=gameObject.GetComponent<UILabel>();

	}

	void LabelAnime(){
		ts.enabled=true;
		ts.ResetToBeginning();
		ts.PlayForward();
	}

	public void OnTweened(){
		if(ts.direction==AnimationOrTween.Direction.Forward){
			
			ts.PlayReverse();
		}else if(ts.direction==AnimationOrTween.Direction.Reverse){
			
			ts.enabled=false;

		}
	}

	public void SetNum(int num){
		label.text=num.ToString();
		LabelAnime();
	}

	public void SetNum(string str){
		Debug.Log("SetNum "+str+" "+gameObject.name);
		label.text=str;
		LabelAnime();
	}
}
