using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colorful;
using System;

/// <summary>
/// Ranking_PUの説明
/// </summary>
public class Ranking_PU : MonoBehaviour {

	#region  メンバ変数
	public GameObject btnBG;
	public GameObject container;
	public UILabel tittleLb;
	public Texture2D unkownUser;
	public RankingDataHolder userData;
	public UILabel previousRankData;
	public int howmany=20;
	public string[] tittleKeys=new string[]{"RankAvr","RankTop","RankKill"};
	public GameObject nodata;

	#endregion


	int currentBoad=0;


	void Start(){
		
	}
		
	#region  Public関数
	public void Show(){
		AudioController.Play("open");
		btnBG.SetActive(true);
		container.SetActive(true);
		LoadRanking();
		if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.HideBanner();
	}

	public void OnClose(){
		AudioController.Play("popup");
		btnBG.SetActive(false);
		container.SetActive(false);
		if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.ShowBanner();
	}

	public void OnNext(){
		Debug.Log("OnNext");
		if(isLoading)return;
		currentBoad++;
		if(currentBoad>2){
			currentBoad=0;
		}
		LoadRanking();

	}
	public void OnPrevious(){
		Debug.Log("OnPrevious");
		if(isLoading)return;
		currentBoad--;
		if(currentBoad<0){
			currentBoad=2;
		}
		LoadRanking();

	}


	GPScore playerScore;
	public void Callback_PlayerScoreUpdatedEvent(GPScore score){
		playerScore=score;
		loadedPlayerRankingData=true;
	
	}
	GPScore playerPreviousScore;
	public void Callback_PlayerPreviousScoreUpdatedEvent(GPScore score){
		playerPreviousScore=score;
		loadedPlayerPrevRankingData=true;

	}
	LeaderboadScore scores;
	public void Callback_scoreUpdatedEvent(LeaderboadScore scores){
		scores=scores;
		loadedRankingData=true;

	}




	#endregion
	

	#region  メンバ関数

	void LoadRanking(){
		if(isLoading)return;
		tittleLb.text=Localization.Get(tittleKeys[currentBoad]);
		PSGUI.WaitHUD.guiWait.Show(11,"Fetching");
		loadedRankingData=false;
		loadedPlayerRankingData=false;
		loadedPlayerPrevRankingData=false;
		StartCoroutine(RankingLoader());

	}
	bool isLoading=false;

	bool loadedRankingData=false;
	bool loadedPlayerRankingData=false;
	bool loadedPlayerPrevRankingData=false;

	IEnumerator RankingLoader(){
		isLoading=true;
		// set User data
		if(!PS_Plugin.Instance.readerboadListener.isLogin()){
			
			yield return new WaitForSeconds(1.0f);
			nodata.SetActive(true);
			PSGUI.WaitHUD.guiWait.Hide();
			isLoading=false;
			yield break;
		}

		loadedRankingData=false;
		loadedPlayerRankingData=false;
		loadedPlayerPrevRankingData=false;


		playerScore=null;
		playerPreviousScore=null;
		scores=null;


		//ユーザーの先月のランクを取得する
		string boadID=GetBoadID(currentBoad,true);


		if(boadID!=""){
			PS_Plugin.Instance.readerboadListener.LoadPlayerRankData(GPBoardTimeSpan.ALL_TIME,boadID,Callback_PlayerScoreUpdatedEvent);
			while(!loadedPlayerPrevRankingData){
				yield return null;
			}
			previousRankData.text=playerPreviousScore!=null?playerPreviousScore.Rank.ToString():"/";

		}

		yield return null;

		//ユーザーの今月のランクを取得する
		boadID=GetBoadID(currentBoad,false);


		if(boadID!=""){
			//まずユーザーの今月のランクを取得する
			PS_Plugin.Instance.readerboadListener.LoadPlayerRankData(GPBoardTimeSpan.ALL_TIME,boadID,Callback_PlayerScoreUpdatedEvent);

			while(!loadedPlayerRankingData){
				yield return null;
			}

			string rank=playerScore!=null?playerScore.Rank.ToString():"/";
			Color rankColor=playerScore!=null?GetColorForRank(playerScore.Rank):Color.white;
			string score=playerScore!=null?LogScoreToString(playerScore.LongScore):"--";
			Texture2D icon=PS_Plugin.Instance.readerboadListener.getPlayerIcon()!=null?PS_Plugin.Instance.readerboadListener.getPlayerIcon():unkownUser;
			string name=PS_Plugin.Instance.readerboadListener.userName_string==""?"You":PS_Plugin.Instance.readerboadListener.userName_string;


			userData.SetUser(rank,rankColor,score,icon,name);
		}


		yield return null;

		if(boadID!=""){
			//まずユーザーの今月のランクを取得する
			PS_Plugin.Instance.readerboadListener.LoadReaderBoadData(GPBoardTimeSpan.ALL_TIME,howmany,boadID,false,Callback_scoreUpdatedEvent);

			while(!loadedRankingData){
				yield return null;
			}
			SetRankingDatas();

		}

		yield return new WaitForSeconds(1.0f);

		PSGUI.WaitHUD.guiWait.Hide();
		isLoading=false;
	}

	public Color[] colorForRank;
	Color GetColorForRank(int rank){
		if(rank<colorForRank.Length){
			return colorForRank[rank];
		}
		return Color.white;
	}
	string LogScoreToString(long score){
		return score.ToString();
	}


	public UIGrid grid;
	public UIScrollView scroll;
	public List<RankingDataHolder> dataHolderList=new List<RankingDataHolder>();
	public GameObject holderbj;

	void SetRankingDatas(){

		dataHolderList.Clear();

		if(scores==null){
			TrimItems(dataHolderList.Count-0);
			//何もないの表示
			nodata.SetActive(true);

			return;
		}

		int count=0;
		for(int i=0;i<scores.names.Length;i++){
			if(scores.hasData[i]){
				count++;
			}
		}
		TrimItems(dataHolderList.Count-count);

		if(count==0){
			nodata.SetActive(false);
			return;
		}
		//ここでデータをセットする。

		string rank="/";
		Color rankColor=Color.white;
		string score="--";
		Texture2D icon=unkownUser;
		string name="--";

		int kensu=0;
		foreach(RankingDataHolder rh in dataHolderList){
			rank=(kensu+1).ToString();
			rankColor=GetColorForRank(kensu);
			score=scores.scores[kensu]!=-1?LogScoreToString(scores.scores[kensu]):"--";
			icon=scores.icon[kensu]!=null?scores.icon[kensu]:unkownUser;
			name=scores.names[kensu]==""?"Unkown User":scores.names[kensu];

			rh.SetUser(rank,rankColor,score,icon,name);
			kensu++;
		}

		Debug.Log("ランキングデータセット完了 件数："+kensu);

		grid.Reposition();
		scroll.ResetPosition();
	}


	void AddList(){
		GameObject go=GameObject.Instantiate(holderbj,null) as GameObject;
		go.transform.parent=grid.transform;
		go.transform.localScale=Vector3.one;
		go.transform.localPosition=Vector3.one;
		go.transform.localRotation=Quaternion.identity;

		RankingDataHolder si;
		si=go.GetComponent<RankingDataHolder>();
		grid.Reposition();
	}

	void TrimItems(int sabun){

		if(sabun<0){
			//足りない
			for(int i=0;i<Mathf.Abs(sabun);i++){
				AddList();
			}

		}else if(sabun>0){
			//余り
			for(int i=0;i<sabun;i++){
				if(dataHolderList[0]!=null){
					dataHolderList[0].KillSelf();
					dataHolderList.RemoveAt(0);
				}
			}
		}
	}


	string GetBoadID(int boadNum,bool isPrevious){

		int month=isPrevious?DateTime.Now.Month-1 :DateTime.Now.Month;
		return PSParams.AppData.GetRankingID(month,PSParams.AppData.RankingTittels[boadNum]);
	}
	#endregion
}
