using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PS_GUI_Cover))]
public class PS_GUI_Cover : MonoBehaviour {

	BoxCollider colliderObj;

	void Awake(){
		colliderObj=GetComponent<BoxCollider>();
		isCovered=false;
		SetCollider(false);
		ta.gameObject.GetComponent<UISprite>().alpha=0.0f;
	}

	void SetCollider(bool isOn){
		colliderObj.enabled=isOn;
	}

	bool isCovered=false;
	public void Cover(){
		Debug.Log("Cover");
		if(!isCovered){
			isCovered=true;
			SetCollider(true);

		}else{
			isCovered=true;
			ta.gameObject.GetComponent<UISprite>().alpha=0.0f;
			SetCollider(true);
		}
	}
	public void CoverWithBlackMask(){
		Debug.Log("CoverWithBlackMask");
		ta.gameObject.SetActive(true);
		if(!isCovered){
			isCovered=true;
			ta.PlayForward();
		}else{
			
			isCovered=true;
			ta.ResetToBeginning();
			ta.PlayForward();
		}
	}

	public void Uncover(){
		Debug.Log("Uncover");
		if(isCovered){
			if(ta.gameObject.GetComponent<UISprite>().alpha>0.5f){
				ta.PlayReverse();
			}else{
				SetCollider(false);
			}

		}
	}

	public TweenAlpha ta;

	public void OnTweened(){
		if(ta.direction==AnimationOrTween.Direction.Forward){
			
			SetCollider(true);
		}else if(ta.direction==AnimationOrTween.Direction.Reverse){
			isCovered=false;
			SetCollider(false);
			ta.gameObject.SetActive(false);
		}
	}



}
