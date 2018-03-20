using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// RankingBtnの説明
/// </summary>
public class RankingBtn : MonoBehaviour {

	public Ranking_PU rankingMenu;
	public UISprite sp;
	public UILabel lb;
	void SetUpdate(bool isOn,int num){
		sp.enabled=isOn;
		lb.enabled=isOn;
		lb.text=num.ToString();
	}

	bool isLoading=false;
	bool loadedRankingData=false;

	void Start(){
		if(PS_Plugin.Instance.readerboadListener.isLogin()){
			StartCoroutine(GetUpdate());
		}else{
			SetUpdate(false,0);
		}
	}
	IEnumerator GetUpdate(){
		isLoading=true;
		int currentBoad=0;
		int infoNum=0;

		string boadID=GetBoadID(currentBoad,false);

		if(boadID!=""){
			//まずユーザーの今月のランクを取得する
			loadedRankingData=false;

			PS_Plugin.Instance.readerboadListener.LoadPlayerRankData(GPBoardTimeSpan.ALL_TIME,boadID,Callback_PlayerScoreUpdatedEvent);

			while(!loadedRankingData){
				yield return null;
			}
			if(playerScore!=null){
				if(DataManager.Instance.gameData.rankingRanks[currentBoad]!=playerScore.Rank){
					DataManager.Instance.gameData.rankingRanks[currentBoad]=playerScore.Rank;
					infoNum++;
				}
			}
		}


		yield return null;

		currentBoad=1;
		boadID=GetBoadID(currentBoad,false);
		if(boadID!=""){
			//まずユーザーの今月のランクを取得する
			loadedRankingData=false;
			PS_Plugin.Instance.readerboadListener.LoadPlayerRankData(GPBoardTimeSpan.ALL_TIME,boadID,Callback_PlayerScoreUpdatedEvent);

			while(!loadedRankingData){
				yield return null;
			}
			if(playerScore!=null){
				if(DataManager.Instance.gameData.rankingRanks[currentBoad]!=playerScore.Rank){
					DataManager.Instance.gameData.rankingRanks[currentBoad]=playerScore.Rank;
					infoNum++;
				}
			}
		}


		yield return null;

		currentBoad=2;
		boadID=GetBoadID(currentBoad,false);
		if(boadID!=""){
			//まずユーザーの今月のランクを取得する
			loadedRankingData=false;
			PS_Plugin.Instance.readerboadListener.LoadPlayerRankData(GPBoardTimeSpan.ALL_TIME,boadID,Callback_PlayerScoreUpdatedEvent);

			while(!loadedRankingData){
				yield return null;
			}
			if(playerScore!=null){
				if(DataManager.Instance.gameData.rankingRanks[currentBoad]!=playerScore.Rank){
					DataManager.Instance.gameData.rankingRanks[currentBoad]=playerScore.Rank;
					infoNum++;
				}
			}
		}


		yield return null;

		if(infoNum==0){
			SetUpdate(false,infoNum);
		}else{
			SetUpdate(true,infoNum);
		}
		DataManager.Instance.SaveAll();
		isLoading=false;
	}

	GPScore playerScore;
	public void Callback_PlayerScoreUpdatedEvent(GPScore score){
		playerScore=score;
		loadedRankingData=true;

	}


	string GetBoadID(int boadNum,bool isPrevious){

		int month=isPrevious?DateTime.Now.Month-1 :DateTime.Now.Month;
		return PSParams.AppData.GetRankingID(month,PSParams.AppData.RankingTittels[boadNum]);
	}

	#region  メンバ関数
		void OnClick() {
		if(isLoading)return;
			rankingMenu.Show();
			return;
			Debug.Log("OnClick");
			if(PS_Plugin.Instance.readerboadListener.isLogin()){
				//PS_Plugin.Instance.readerboadListener.Open();
				rankingMenu.Show();
			}else{
				PSPhoton.LobbyManager.instance.info.Log(
				Application.systemLanguage == SystemLanguage.Japanese? "サーバーエラーにより取得できませんでした" :"Ranking retrieve failled");
			}
		}
	#endregion
}
