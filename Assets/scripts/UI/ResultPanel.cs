using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colorful;
public class ResultPanel : MonoBehaviour {

	public GaussianBlur blurEffect;
	public PS_GUI_Cover cover;

	public void ShowResult(float time,int killNum,int rank,int playerNumber,shipControl playerShip){
		isShowing=true;

		cover.Cover();
		//全参加者数
		int players=playerNumber;

		//プレイヤの順位
		string playerRank=rank.ToString();
		//プレイヤの生存時間
		int minutes = Mathf.FloorToInt(time / 60F);
		int seconds = Mathf.FloorToInt(time - minutes * 60);
		string aliveTime= string.Format("{0:00}:{1:00}", minutes, seconds);
		//プレイヤのキル数
		string kills=killNum.ToString();

		DataManager.Instance.gameData.isConnectingRoom=false;
		DataManager.Instance.SaveAll();
		SetUserData(playerShip.playerData.countlyCode,playerShip.playerData.userName,aliveTime,playerRank,kills,"/"+players.ToString());
		Invoke("Show",3.0f);
	}

	public void Show(){
		blurEffect.enabled=true;
		ta_panel.PlayForward();
	}

	public UILabel userName;
	public UILabel userTime;
	public UILabel userRank;
	public UILabel userRankBG;
	public UILabel userKill;
	public UISprite userFlag;
	void SetUserData(string flag,string name,string time,string rank,string kill,string players){
		userName.text=name;
		userTime.text=time;
		userRank.text=rank;
		userRankBG.text=players;
		userKill.text=kill;
		userFlag.spriteName=flag;
	}



	public void CloseResult(){
		ta_panel.PlayReverse();
	}


	public PSGUI.SceneFader sceneFader;
	public void OnCliclMain(){
		sceneFader.FadeOut(BackToMain,true);
	}

	public void BackToMain(){
		PSPhoton.GameManager.instance.BackToMain();
	}


	public bool isShowing=false;

	public TweenAlpha ta_panel;
	public void OnTweenPanel(){
		if(ta_panel.direction==AnimationOrTween.Direction.Forward){
			NGUITools.SetActive(backButton,true);
		}else if(ta_panel.direction==AnimationOrTween.Direction.Reverse){
			isShowing=false;
		}
	}

	public GameObject backButton;

}
