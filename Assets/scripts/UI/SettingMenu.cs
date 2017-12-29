using UnityEngine;
using System.Collections;

public class SettingMenu : MonoBehaviour {

	public PS_GUI_Toggle toggleSE;
	public PS_GUI_Toggle toggleBGM;

	public bool toggleSEFlag=false;
	public bool toggleBGMFlag=false;


	public void SetSettignValues(bool se,bool bgm){
		toggleSEFlag=se;
		toggleBGMFlag=bgm;
		toggleSE.SetState(toggleSEFlag);
		toggleBGM.SetState(toggleBGMFlag);
	}

	void Start(){
		
		toggleSE.SetVisible(false);
		toggleBGM.SetVisible(false);

	}

	bool settingState=false;
	public void OnClickSettting(){
		if(!settingState){
			settingState=true;
			OpenSetting();
		}else{
			settingState=false;
			CloseSetting();
		}
	}

	public void OnClickSE(){
		toggleSEFlag=!toggleSEFlag;
		toggleSE.SetState(toggleSEFlag);
	}
	public void OnClickBGM(){
		toggleBGMFlag=!toggleBGMFlag;
		toggleBGM.SetState(toggleBGMFlag);
	}

	void OpenSetting(){
		bgScale.PlayForward();
	}
	void CloseSetting(){
		bgScale.PlayReverse();
	}
	public TweenAlpha bgScale;

	public void OnBGTweened(){
		if(bgScale.direction==AnimationOrTween.Direction.Forward){
			
			toggleSE.SetVisible(true);
			toggleBGM.SetVisible(true);
		}else if(bgScale.direction==AnimationOrTween.Direction.Reverse){

			toggleSE.SetVisible(false);
			toggleBGM.SetVisible(false);
		}
	}
}
