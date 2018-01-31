using UnityEngine;
using System.Collections;

public class PS_GUI_HP : PS_GUI_HPSlider {

	public Color colorOnKaihuku;
	public Color colorOnDamage;
	public Color colorNormal;

	public override void OnUpdate(){
		//違う場合には合わせる
		if(slider.fillAmount != subSlider.fillAmount){
			if(slider.fillAmount>subSlider.fillAmount){
				//回復時
				subSlider.fillAmount=Mathf.Lerp(subSlider.fillAmount,slider.fillAmount,tweenTime);
				if(subSlider.color!=colorNormal)subSlider.color=colorNormal;
				if(slider.color!=colorOnKaihuku)slider.color=colorOnKaihuku;
				if(subSlider.depth!=4)subSlider.depth=4;
			}else{
				//ダメージ時
				subSlider.fillAmount=Mathf.Lerp(subSlider.fillAmount,slider.fillAmount,tweenTime);
				if(subSlider.color!=colorOnDamage)subSlider.color=colorOnDamage;
				if(slider.alpha!=1.0f)slider.alpha=1.0f;
				if(subSlider.depth!=2)subSlider.depth=2;
			}

		}else{
			//通常時
			if(subSlider.color!=colorNormal)subSlider.color=colorNormal;
			if(slider.alpha!=1.0f)slider.alpha=1.0f;
			if(subSlider.depth!=2)subSlider.depth=2;
		}


		//HP残りが少なくなると点滅させる
		if(!ta.enabled && initAlpa!=slider.alpha){
			//
			slider.alpha=initAlpa;
		}


		if(slider.fillAmount>=startBlinkAt){
			SetTween(false);
		}else{
			SetTween(true);
		}
	}
}
