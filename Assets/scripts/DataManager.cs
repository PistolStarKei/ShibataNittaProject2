using UnityEngine;
using System.Collections;
using PSParams;

[System.Serializable]
public class EnvData{
	public bool toggleSE=true;
	public bool toggleBGM=true;
	public string startTime="";
	public string serverRegion ="";
	public bool isTutorialed=false;
}

[System.Serializable]
public class GameData{
	public int shipType =0;
	public int shipColor=0;
	public string username ="UNKNOWN";
	public bool isConnectingRoom =false;
	public string lastRoomName ="";
	public string country ="";
	public string lastTime ="";
	public int gameTickets =0;
	public float timeForNextTickets =0.0f;
	public string userID ="";
	public int tweetNum=0;
	public bool[] shipAvaillable1=GameParameters.defaultAvaillabilityShip["Ship1"];
	public bool[] shipAvaillable2=GameParameters.defaultAvaillabilityShip["Ship2"];
	public bool[] shipAvaillable3=GameParameters.defaultAvaillabilityShip["Ship3"];
	public bool[] shipAvaillable4=GameParameters.defaultAvaillabilityShip["Ship4"];
	public bool[] shipAvaillable5=GameParameters.defaultAvaillabilityShip["Ship5"];
	public bool[] shipAvaillable6=GameParameters.defaultAvaillabilityShip["Ship6"];
	public bool[] shipAvaillable7=GameParameters.defaultAvaillabilityShip["Ship7"];
	public bool[] shipAvaillable8=GameParameters.defaultAvaillabilityShip["Ship8"];
	public int playNum =0;
	public int rankingKillNum =0;
	public int rankingTopNum =0;
	public int rankingTotalPlay =0;
	public int rankingTotalRank =0;
	public float rankingAvrRank =0;
	public bool isReviewd =false;
	public int[] rankingRanks=new int[3]{0,0,0};
	public string loginDay ="";
}

public class DataManager : PS_SingletonBehaviour<DataManager> {

	public bool isPersistantBetweenScenes=false;
	public const string filename = "SavedData.es?encrypt=true&password=pass&tag=";

	public EnvData envData=new EnvData();
	public GameData gameData=new GameData();

	public bool debugDelete=false;

	void Awake(){
		
		if(isPersistantBetweenScenes){

			DontDestroyOnLoad(this.gameObject);

			if(this != Instance)
			{
				DestroyAll();
				return;
			}

		}


		#if UNITY_EDITOR
		if(debugDelete){
			ES2.Delete(filename+"isTutorialed");	
		}
		#endif

		if(!ES2.Exists(filename+"isTutorialed")){
			
			InitData();
			LoadData();
		}else{
			LoadData();

			//フラグ配列をマッチさせる
			CheckAndMatchFlagArrayLengths();
			SaveAll();
		}



		if(Application.systemLanguage==SystemLanguage.Japanese){
			Localization.language="Japanese";
		}else{
			Localization.language="English";
		}
		Debug.Log(""+Localization.language);

	}


	public void CheckTweetNum(){
		if(TimeManager.Instance!=null){
			if(!TimeManager.Instance.ISSameDayLogin(TimeManager.StringToDateTime(gameData.lastTime))){
				gameData.tweetNum=0;
				gameData.playNum++;
				SaveAll();
			}

		}
	}
	public void LoadData(){
		envData.toggleSE=ES2.Load<bool>(filename+"toggleSE");
		envData.toggleBGM=ES2.Load<bool>(filename+"toggleBGM");
		envData.startTime=ES2.Load<string>(filename+"startTime");
		envData.serverRegion=ES2.Load<string>(filename+"serverRegion");
		envData.isTutorialed=ES2.Load<bool>(filename+"isTutorialed");

		gameData.shipType=ES2.Load<int>(filename+"shipType");
		gameData.username=ES2.Load<string>(filename+"username");
		gameData.isConnectingRoom=ES2.Load<bool>(filename+"isConnectingRoom");
		gameData.country=ES2.Load<string>(filename+"country");
		gameData.lastTime=ES2.Load<string>(filename+"lastTime");

		gameData.gameTickets=ES2.Load<int>(filename+"gameTickets");
		gameData.timeForNextTickets=ES2.Load<float>(filename+"timeForNextTickets");

		gameData.userID=ES2.Load<string>(filename+"userID");
		gameData.lastRoomName=ES2.Load<string>(filename+"lastRoomName");
		gameData.shipColor=ES2.Load<int>(filename+"shipColor");
		gameData.tweetNum=ES2.Load<int>(filename+"tweetNum");


		gameData.shipAvaillable1=ES2.LoadArray<bool>(filename+"shipAvaillable1");
		gameData.shipAvaillable2=ES2.LoadArray<bool>(filename+"shipAvaillable2");
		gameData.shipAvaillable3=ES2.LoadArray<bool>(filename+"shipAvaillable3");
		gameData.shipAvaillable4=ES2.LoadArray<bool>(filename+"shipAvaillable4");
		gameData.shipAvaillable5=ES2.LoadArray<bool>(filename+"shipAvaillable5");
		gameData.shipAvaillable6=ES2.LoadArray<bool>(filename+"shipAvaillable6");
		gameData.shipAvaillable7=ES2.LoadArray<bool>(filename+"shipAvaillable7");
		gameData.shipAvaillable8=ES2.LoadArray<bool>(filename+"shipAvaillable8");



		gameData.rankingKillNum=ES2.Load<int>(filename+"rankingKillNum");
		gameData.rankingTopNum=ES2.Load<int>(filename+"rankingTopNum");
		gameData.rankingAvrRank=ES2.Load<float>(filename+"rankingAvrRank");

		gameData.rankingTotalPlay=ES2.Load<int>(filename+"rankingTotalPlay");
		gameData.rankingTotalRank=ES2.Load<int>(filename+"rankingTotalRank");
		gameData.playNum=ES2.Load<int>(filename+"playNum");
		gameData.isReviewd=ES2.Load<bool>(filename+"isReviewd");

		gameData.rankingRanks=ES2.LoadArray<int>(filename+"rankingRanks");

		gameData.loginDay=ES2.Load<string>(filename+"loginDay");



	}

	public void SaveAll(){
		ES2.Save(envData.toggleSE,filename+"toggleSE");
		ES2.Save(envData.toggleBGM,filename+"toggleBGM");
		ES2.Save(envData.startTime,filename+"startTime");
		ES2.Save(envData.serverRegion,filename+"serverRegion");
		ES2.Save(envData.isTutorialed,filename+"isTutorialed");

		ES2.Save(gameData.shipType,filename+"shipType");
		ES2.Save(gameData.username,filename+"username");
		ES2.Save(gameData.isConnectingRoom,filename+"isConnectingRoom");
		ES2.Save(gameData.country,filename+"country");
		ES2.Save(gameData.lastTime,filename+"lastTime");

		ES2.Save(gameData.gameTickets,filename+"gameTickets");
		ES2.Save(gameData.timeForNextTickets,filename+"timeForNextTickets");
		ES2.Save(gameData.userID,filename+"userID");
		ES2.Save(gameData.lastRoomName,filename+"lastRoomName");
		ES2.Save(gameData.shipColor,filename+"shipColor");
		ES2.Save(gameData.tweetNum,filename+"tweetNum");

		ES2.Save(gameData.shipAvaillable1,filename+"shipAvaillable1");
		ES2.Save(gameData.shipAvaillable2,filename+"shipAvaillable2");
		ES2.Save(gameData.shipAvaillable3,filename+"shipAvaillable3");
		ES2.Save(gameData.shipAvaillable4,filename+"shipAvaillable4");
		ES2.Save(gameData.shipAvaillable5,filename+"shipAvaillable5");
		ES2.Save(gameData.shipAvaillable6,filename+"shipAvaillable6");
		ES2.Save(gameData.shipAvaillable7,filename+"shipAvaillable7");
		ES2.Save(gameData.shipAvaillable8,filename+"shipAvaillable8");

		ES2.Save(gameData.rankingKillNum,filename+"rankingKillNum");
		ES2.Save(gameData.rankingTopNum,filename+"rankingTopNum");
		ES2.Save(gameData.rankingAvrRank,filename+"rankingAvrRank");
		ES2.Save(gameData.rankingTotalPlay,filename+"rankingTotalPlay");

		ES2.Save(gameData.rankingTotalRank,filename+"rankingTotalRank");

		ES2.Save(gameData.playNum,filename+"playNum");
		ES2.Save(gameData.isReviewd,filename+"isReviewd");

		ES2.Save(gameData.rankingRanks,filename+"rankingRanks");
		ES2.Save(gameData.loginDay,filename+"loginDay");
	}

	private void DestroyAll(){
		foreach (Transform childTransform in gameObject.transform) Destroy(childTransform.gameObject);
		Destroy(gameObject);
	}

	private void InitData(){
		ES2.Save(true,filename+"toggleSE");
		ES2.Save(true,filename+"toggleBGM");
		ES2.Save(TimeManager.GetCurrentTime(),filename+"startTime");
		ES2.Save(Countly.GetRegion(Application.systemLanguage),filename+"serverRegion");

		ES2.Save(0,filename+"shipType");
		ES2.Save(Application.systemLanguage == SystemLanguage.Japanese?"タップで名前を入力":"Tap here to input",filename+"username");
		ES2.Save(false,filename+"isConnectingRoom");

		ES2.Save(Countly.ToCountryCode(Application.systemLanguage),filename+"country");
		ES2.Save(TimeManager.GetCurrentTime(),filename+"lastTime");
		ES2.Save(PSParams.GameParameters.DefaultTicketsNum,filename+"gameTickets");
		ES2.Save(-1.0f,filename+"timeForNextTickets");
		ES2.Save(GetUUID (),filename+"userID");
		ES2.Save("",filename+"lastRoomName");
		ES2.Save(0,filename+"shipColor");
		ES2.Save(0,filename+"tweetNum");

		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship1"],filename+"shipAvaillable1");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship2"],filename+"shipAvaillable2");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship3"],filename+"shipAvaillable3");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship4"],filename+"shipAvaillable4");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship5"],filename+"shipAvaillable5");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship6"],filename+"shipAvaillable6");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship7"],filename+"shipAvaillable7");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship8"],filename+"shipAvaillable8");

		ES2.Save(0,filename+"rankingKillNum");
		ES2.Save(0,filename+"rankingTopNum");
		ES2.Save(0f,filename+"rankingAvrRank");
		ES2.Save(0,filename+"rankingTotalPlay");
		ES2.Save(0,filename+"rankingTotalRank");

		ES2.Save(0,filename+"playNum");
		ES2.Save(false,filename+"isReviewd");

		ES2.Save(new int[3]{0,0,0},filename+"rankingRanks");
		ES2.Save(TimeManager.GetCurrentTime(),filename+"loginDay");

		ES2.Save(false,filename+"isTutorialed");
	}
		
	private void DeleteAll(){
		#if UNITY_EDITOR
			ES2.DeleteDefaultFolder();
		#endif
	}
	void OnApplicationQuit(){
		DataManager.Instance.gameData.lastTime=TimeManager.GetCurrentTime();
		DataManager.Instance.SaveAll();
	}

	public bool IsTweetDay(){
		int keika=TimeManager.GetKeikaDaysSinceLast(TimeManager.StringToDateTime(gameData.loginDay));

		if(keika>0 && keika%7==0){
			//ログインから7日後
			return true;
		}

		return false;

	}

	public static string GetUUID ()
	{
			// Global Unique IDentifier
			System.Guid guid = System.Guid.NewGuid ();
			string uuid = guid.ToString ().Replace("-", "");
			Debug.Log (uuid);
			return uuid;
	}


	public bool[] GetAvaillableFlagFromData(int shipNum){
		bool[] flags=new bool[0];
		switch(shipNum){
			case 0:
				flags=gameData.shipAvaillable1;
				break;
			case 1:
				flags=gameData.shipAvaillable2;
				break;
			case 2:
				flags=gameData.shipAvaillable3;
				break;
			case 3:
				flags=gameData.shipAvaillable4;
				break;
			case 4:
				flags=gameData.shipAvaillable5;
				break;
			case 5:
				flags=gameData.shipAvaillable6;
				break;
			case 6:
				flags=gameData.shipAvaillable7;
				break;
			case 7:
				flags=gameData.shipAvaillable8;
				break;
		}
		return flags;
	}

	public void SetShipPurchase(int shipNum,int color,bool flag){


		switch(shipNum){
			case 0:
				gameData.shipAvaillable1[color]=flag;
				break;
			case 1:
				gameData.shipAvaillable2[color]=flag;
				break;
			case 2:
				gameData.shipAvaillable3[color]=flag;
				break;
			case 3:
				gameData.shipAvaillable4[color]=flag;
				break;
			case 4:
				gameData.shipAvaillable5[color]=flag;
				break;
			case 5:
				gameData.shipAvaillable6[color]=flag;
				break;
			case 6:
				gameData.shipAvaillable7[color]=flag;
				break;
			case 7:
				gameData.shipAvaillable8[color]=flag;
			break;
		}

		this.SaveAll();
	}

	void CheckAndMatchFlagArrayLengths(){
		Debug.LogWarning("ここで配列アシストをする");
		gameData.shipAvaillable1=GetMergedBool(gameData.shipAvaillable1,GameParameters.defaultAvaillabilityShip["Ship1"]);
		gameData.shipAvaillable2=GetMergedBool(gameData.shipAvaillable2,GameParameters.defaultAvaillabilityShip["Ship2"]);
		gameData.shipAvaillable3=GetMergedBool(gameData.shipAvaillable3,GameParameters.defaultAvaillabilityShip["Ship3"]);
		gameData.shipAvaillable4=GetMergedBool(gameData.shipAvaillable4,GameParameters.defaultAvaillabilityShip["Ship4"]);
		gameData.shipAvaillable5=GetMergedBool(gameData.shipAvaillable5,GameParameters.defaultAvaillabilityShip["Ship5"]);
		gameData.shipAvaillable6=GetMergedBool(gameData.shipAvaillable6,GameParameters.defaultAvaillabilityShip["Ship6"]);
		gameData.shipAvaillable7=GetMergedBool(gameData.shipAvaillable7,GameParameters.defaultAvaillabilityShip["Ship7"]);
		gameData.shipAvaillable8=GetMergedBool(gameData.shipAvaillable8,GameParameters.defaultAvaillabilityShip["Ship8"]);

	}
	bool[] GetMergedBool(bool[] oldFlags,bool[] newFlags){
		bool[] temp=newFlags;
		if(oldFlags.Length!=newFlags.Length){

			for(int i=0;i<temp.Length;i++){
				if(i<oldFlags.Length)temp[i]=oldFlags[i];
			}
			return temp;
		}

		return oldFlags;
	}


}
