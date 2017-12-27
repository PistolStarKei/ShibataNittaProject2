using UnityEngine;
using System.Collections;

public class PS_GUI_HPSlider : MonoBehaviour {

	public UISprite slider;
	public UISprite subSlider;
	public TweenAlpha ta;

	void SetTween(bool isOn){
		ta.enabled=isOn;

	}
	float initAlpa=0.0f;
	public bool isStartTween=false;
	void Start(){
		if(isStartTween){
			slider.fillAmount=0.0f;
			subSlider.fillAmount=0.0f;
			SetHPVal(1.0f);
		}else{
			slider.fillAmount=1.0f;
			subSlider.fillAmount=1.0f;
		}
		initAlpa=slider.alpha;
		SetTween(false);
	}

	public float startBlinkAt=0.1f;
	public float tweenTime=0.1f;
	void Update(){
		if(slider.fillAmount != subSlider.fillAmount){
			subSlider.fillAmount=Mathf.Lerp(subSlider.fillAmount,slider.fillAmount,tweenTime);
		}

		if(!ta.enabled && initAlpa!=slider.alpha){
			slider.alpha=initAlpa;
		}
		if(slider.fillAmount>=startBlinkAt){
			SetTween(false);
		}else{
			SetTween(true);
		}
	}

	void SetHPVal(float val){
		if(val>1.0f)val=1.0f;
		if(val<0.0f)val=0.0f;

		slider.fillAmount=val;

		if(slider.fillAmount==startBlinkAt)return;
		if(slider.fillAmount>=startBlinkAt){
			SetTween(false);
		}else{
			SetTween(true);
		}
	}

	public void AddVal(float addVal){
		SetHPVal(slider.fillAmount+addVal);
	}
	public void MinusVal(float minusVal){
		SetHPVal(slider.fillAmount-minusVal);
	}

}
