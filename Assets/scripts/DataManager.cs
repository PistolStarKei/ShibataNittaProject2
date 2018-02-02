using UnityEngine;
using System.Collections;
using PSParams;

[System.Serializable]
public class EnvData{
	public bool toggleSE=true;
	public bool toggleBGM=true;
	public string startTime="";

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

	public int rankingKillNum =0;
	public int rankingTopNum =0;
	public int rankingTotalPlay =0;
	public int rankingTotalRank =0;
	public float rankingAvrRank =0;

}

public class DataManager : PS_SingletonBehaviour<DataManager> {

	public bool isPersistantBetweenScenes=false;
	public const string filename = "SavedData.txt";

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
			ES2.Delete(filename+"?tag=rankingKillNum");	
		}
		#endif

		if(!ES2.Exists(filename+"?tag=rankingKillNum")){
			
			InitData();
			LoadData();
		}else{
			LoadData();

			//フラグ配列をマッチさせる
			CheckAndMatchFlagArrayLengths();
			SaveAll();
		}

		if(TimeManager.Instance!=null){
			if(TimeManager.Instance.ISSameDayLogin(TimeManager.StringToDateTime(gameData.lastTime))){
				gameData.tweetNum=0;
				SaveAll();
			}

		}

		if(Application.systemLanguage==SystemLanguage.Japanese){
			Localization.language="Japanese";
		}else{
			Localization.language="English";
		}
		Debug.Log(""+Localization.language);

	}


	public void LoadData(){
		envData.toggleSE=ES2.Load<bool>(filename+"?tag=toggleSE");
		envData.toggleBGM=ES2.Load<bool>(filename+"?tag=toggleBGM");
		envData.startTime=ES2.Load<string>(filename+"?tag=startTime");

		gameData.shipType=ES2.Load<int>(filename+"?tag=shipType");
		gameData.username=ES2.Load<string>(filename+"?tag=username");
		gameData.isConnectingRoom=ES2.Load<bool>(filename+"?tag=isConnectingRoom");
		gameData.country=ES2.Load<string>(filename+"?tag=country");
		gameData.lastTime=ES2.Load<string>(filename+"?tag=lastTime");

		gameData.gameTickets=ES2.Load<int>(filename+"?tag=gameTickets");
		gameData.timeForNextTickets=ES2.Load<float>(filename+"?tag=timeForNextTickets");

		gameData.userID=ES2.Load<string>(filename+"?tag=userID");
		gameData.lastRoomName=ES2.Load<string>(filename+"?tag=lastRoomName");
		gameData.shipColor=ES2.Load<int>(filename+"?tag=shipColor");
		gameData.tweetNum=ES2.Load<int>(filename+"?tag=tweetNum");


		gameData.shipAvaillable1=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable1");
		gameData.shipAvaillable2=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable2");
		gameData.shipAvaillable3=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable3");
		gameData.shipAvaillable4=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable4");
		gameData.shipAvaillable5=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable5");
		gameData.shipAvaillable6=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable6");
		gameData.shipAvaillable7=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable7");
		gameData.shipAvaillable8=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable8");



		gameData.rankingKillNum=ES2.Load<int>(filename+"?tag=rankingKillNum");
		gameData.rankingTopNum=ES2.Load<int>(filename+"?tag=rankingTopNum");
		gameData.rankingAvrRank=ES2.Load<float>(filename+"?tag=rankingAvrRank");

		gameData.rankingTotalPlay=ES2.Load<int>(filename+"?tag=rankingTotalPlay");
		gameData.rankingTotalRank=ES2.Load<int>(filename+"?tag=rankingTotalRank");

	}

	public void SaveAll(){
		ES2.Save(envData.toggleSE,filename+"?tag=toggleSE");
		ES2.Save(envData.toggleBGM,filename+"?tag=toggleBGM");
		ES2.Save(envData.startTime,filename+"?tag=startTime");

		ES2.Save(gameData.shipType,filename+"?tag=shipType");
		ES2.Save(gameData.username,filename+"?tag=username");
		ES2.Save(gameData.isConnectingRoom,filename+"?tag=isConnectingRoom");
		ES2.Save(gameData.country,filename+"?tag=country");
		ES2.Save(gameData.lastTime,filename+"?tag=lastTime");

		ES2.Save(gameData.gameTickets,filename+"?tag=gameTickets");
		ES2.Save(gameData.timeForNextTickets,filename+"?tag=timeForNextTickets");
		ES2.Save(gameData.userID,filename+"?tag=userID");
		ES2.Save(gameData.lastRoomName,filename+"?tag=lastRoomName");
		ES2.Save(gameData.shipColor,filename+"?tag=shipColor");
		ES2.Save(gameData.tweetNum,filename+"?tag=tweetNum");

		ES2.Save(gameData.shipAvaillable1,filename+"?tag=shipAvaillable1");
		ES2.Save(gameData.shipAvaillable2,filename+"?tag=shipAvaillable2");
		ES2.Save(gameData.shipAvaillable3,filename+"?tag=shipAvaillable3");
		ES2.Save(gameData.shipAvaillable4,filename+"?tag=shipAvaillable4");
		ES2.Save(gameData.shipAvaillable5,filename+"?tag=shipAvaillable5");
		ES2.Save(gameData.shipAvaillable6,filename+"?tag=shipAvaillable6");
		ES2.Save(gameData.shipAvaillable7,filename+"?tag=shipAvaillable7");
		ES2.Save(gameData.shipAvaillable8,filename+"?tag=shipAvaillable8");

		ES2.Save(gameData.rankingKillNum,filename+"?tag=rankingKillNum");
		ES2.Save(gameData.rankingTopNum,filename+"?tag=rankingTopNum");
		ES2.Save(gameData.rankingAvrRank,filename+"?tag=rankingAvrRank");
		ES2.Save(gameData.rankingTotalPlay,filename+"?tag=rankingTotalPlay");

		ES2.Save(gameData.rankingTotalRank,filename+"?tag=rankingTotalRank");

	}

	private void DestroyAll(){
		foreach (Transform childTransform in gameObject.transform) Destroy(childTransform.gameObject);
		Destroy(gameObject);
	}

	private void InitData(){
		ES2.Save(true,filename+"?tag=toggleSE");
		ES2.Save(true,filename+"?tag=toggleBGM");
		ES2.Save(TimeManager.GetCurrentTime(),filename+"?tag=startTime");


		ES2.Save(0,filename+"?tag=shipType");
		ES2.Save(Application.systemLanguage == SystemLanguage.Japanese?"名無しさん":"Unkown",filename+"?tag=username");
		ES2.Save(false,filename+"?tag=isConnectingRoom");

		ES2.Save(Countly.ToCountryCode(Application.systemLanguage),filename+"?tag=country");
		ES2.Save(TimeManager.GetCurrentTime(),filename+"?tag=lastTime");
		ES2.Save(PSParams.GameParameters.DefaultTicketsNum,filename+"?tag=gameTickets");
		ES2.Save(-1.0f,filename+"?tag=timeForNextTickets");
		ES2.Save(GetUUID (),filename+"?tag=userID");
		ES2.Save("",filename+"?tag=lastRoomName");
		ES2.Save(0,filename+"?tag=shipColor");
		ES2.Save(0,filename+"?tag=tweetNum");

		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship1"],filename+"?tag=shipAvaillable1");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship2"],filename+"?tag=shipAvaillable2");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship3"],filename+"?tag=shipAvaillable3");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship4"],filename+"?tag=shipAvaillable4");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship5"],filename+"?tag=shipAvaillable5");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship6"],filename+"?tag=shipAvaillable6");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship7"],filename+"?tag=shipAvaillable7");
		ES2.Save(GameParameters.defaultAvaillabilityShip["Ship8"],filename+"?tag=shipAvaillable8");

	
		ES2.Save(0,filename+"?tag=rankingKillNum");
		ES2.Save(0,filename+"?tag=rankingTopNum");
		ES2.Save(0f,filename+"?tag=rankingAvrRank");
		ES2.Save(0,filename+"?tag=rankingTotalPlay");
		ES2.Save(0,filename+"?tag=rankingTotalRank");
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
