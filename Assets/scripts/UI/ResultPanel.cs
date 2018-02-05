using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colorful;
public class ResultPanel : MonoBehaviour {

	public GameObject blurEffect;
	public PS_GUI_Cover cover;
	public UILabel gameoverLb;
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


		gameoverLb.text=rank==1?"Total Victory":"Game Over";


		//ランキング関連を
		DataManager.Instance.gameData.rankingKillNum+=killNum;
		long submit=System.Convert.ToInt64(DataManager.Instance.gameData.rankingKillNum);

		PS_Plugin.Instance.readerboadListener.SubmitScore(submit,PSParams.AppData.GetRankingID(System.DateTime.Now.Month,PSParams.AppData.RankingTittels[0]));


		if(rank==1){
			DataManager.Instance.gameData.rankingTopNum+=1;
			submit=System.Convert.ToInt64(DataManager.Instance.gameData.rankingTopNum);
			PS_Plugin.Instance.readerboadListener.SubmitScore(submit,PSParams.AppData.GetRankingID(System.DateTime.Now.Month,PSParams.AppData.RankingTittels[2]));
		}
		DataManager.Instance.gameData.rankingTotalPlay++;
		DataManager.Instance.gameData.rankingTotalRank+=rank;
		DataManager.Instance.gameData.rankingAvrRank=(float)DataManager.Instance.gameData.rankingTotalRank/(float)DataManager.Instance.gameData.rankingTotalPlay;

		//平均順位の送信
		if(DataManager.Instance.gameData.rankingTotalPlay>PSParams.GameParameters.playNumToJoinAvgRanking){
			submit=System.Convert.ToInt64(DataManager.Instance.gameData.rankingAvrRank);
			PS_Plugin.Instance.readerboadListener.SubmitScore(submit,PSParams.AppData.GetRankingID(System.DateTime.Now.Month,PSParams.AppData.RankingTittels[1]));
		}




		DataManager.Instance.SaveAll();

		DataManager.Instance.gameData.isConnectingRoom=false;
		DataManager.Instance.SaveAll();
		SetUserData(playerShip.playerData.countlyCode,playerShip.playerData.userName,aliveTime,playerRank,kills,playerRank+"/"+players.ToString());
		Invoke("Show",3.0f);
	}

	public void Show(){
		NGUITools.SetActive(blurEffect,true);
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
		AudioController.Play("popup");
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
