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
		SetAudioVolume("FX",toggleSEFlag);
		SetAudioVolume("BGM",toggleBGMFlag);
		toggleSE.SetState(toggleSEFlag);
		toggleBGM.SetState(toggleBGMFlag);
	}
	void SetAudioVolume(string category,bool isOn){
		AudioController.SetCategoryVolume(category=="BGM" ? "BGM":"FX", isOn?1.0f:0.0f);
	}

	void Start(){
		


		if(DataManager.Instance)SetSettignValues(DataManager.Instance.envData.toggleSE,DataManager.Instance.envData.toggleBGM);

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
		DataManager.Instance.envData.toggleSE=toggleSEFlag;
		DataManager.Instance.SaveAll();
		SetSettignValues(DataManager.Instance.envData.toggleSE,DataManager.Instance.envData.toggleBGM);

	}
	public void OnClickBGM(){
		toggleBGMFlag=!toggleBGMFlag;
		toggleBGM.SetState(toggleBGMFlag);
		DataManager.Instance.envData.toggleBGM=toggleBGMFlag;
		DataManager.Instance.SaveAll();
		SetSettignValues(DataManager.Instance.envData.toggleSE,DataManager.Instance.envData.toggleBGM);
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
