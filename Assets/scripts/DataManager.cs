using UnityEngine;
using System.Collections;

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
	public bool[] shipAvaillable1=PSParams.GameParameters.defaultAvaillabilityShip1;
	public bool[] shipAvaillable2=PSParams.GameParameters.defaultAvaillabilityShip2;
	public bool[] shipAvaillable3=PSParams.GameParameters.defaultAvaillabilityShip3;
	public bool[] shipAvaillable4=PSParams.GameParameters.defaultAvaillabilityShip4;
	public bool[] shipAvaillable5=PSParams.GameParameters.defaultAvaillabilityShip5;
	public bool[] shipAvaillable6=PSParams.GameParameters.defaultAvaillabilityShip6;
	public bool[] shipAvaillable7=PSParams.GameParameters.defaultAvaillabilityShip7;
	public bool[] shipAvaillable8=PSParams.GameParameters.defaultAvaillabilityShip8;


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
			ES2.Delete(filename+"?tag=startTime");	
		}
		#endif

		if(!ES2.Exists(filename+"?tag=startTime")){
			
			InitData();
			LoadData();
		}else{
			LoadData();

			//フラグ配列をマッチさせる
			CheckAndMatchFlagArrayLengths();
			SaveAll();
		}

	}


	public void LoadData(){
		envData.toggleSE=ES2.Load<bool>(filename+"?tag=toggleSE");
		envData.toggleBGM=ES2.Load<bool>(filename+"?tag=toggleBGM");

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


		gameData.shipAvaillable1=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable1");
		gameData.shipAvaillable2=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable2");
		gameData.shipAvaillable3=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable3");
		gameData.shipAvaillable4=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable4");
		gameData.shipAvaillable5=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable5");
		gameData.shipAvaillable6=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable6");
		gameData.shipAvaillable7=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable7");
		gameData.shipAvaillable8=ES2.LoadArray<bool>(filename+"?tag=shipAvaillable8");

		envData.startTime=ES2.Load<string>(filename+"?tag=startTime");

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


		ES2.Save(gameData.shipAvaillable1,filename+"?tag=shipAvaillable1");
		ES2.Save(gameData.shipAvaillable2,filename+"?tag=shipAvaillable2");
		ES2.Save(gameData.shipAvaillable3,filename+"?tag=shipAvaillable3");
		ES2.Save(gameData.shipAvaillable4,filename+"?tag=shipAvaillable4");
		ES2.Save(gameData.shipAvaillable5,filename+"?tag=shipAvaillable5");
		ES2.Save(gameData.shipAvaillable6,filename+"?tag=shipAvaillable6");
		ES2.Save(gameData.shipAvaillable7,filename+"?tag=shipAvaillable7");
		ES2.Save(gameData.shipAvaillable8,filename+"?tag=shipAvaillable8");



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


		ES2.Save(PSParams.GameParameters.defaultAvaillabilityShip1,filename+"?tag=shipAvaillable1");
		ES2.Save(PSParams.GameParameters.defaultAvaillabilityShip2,filename+"?tag=shipAvaillable2");
		ES2.Save(PSParams.GameParameters.defaultAvaillabilityShip3,filename+"?tag=shipAvaillable3");
		ES2.Save(PSParams.GameParameters.defaultAvaillabilityShip4,filename+"?tag=shipAvaillable4");
		ES2.Save(PSParams.GameParameters.defaultAvaillabilityShip5,filename+"?tag=shipAvaillable5");
		ES2.Save(PSParams.GameParameters.defaultAvaillabilityShip6,filename+"?tag=shipAvaillable6");
		ES2.Save(PSParams.GameParameters.defaultAvaillabilityShip7,filename+"?tag=shipAvaillable7");
		ES2.Save(PSParams.GameParameters.defaultAvaillabilityShip8,filename+"?tag=shipAvaillable8");

	


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



	void CheckAndMatchFlagArrayLengths(){
		Debug.LogWarning("ここで配列アシストをする");
		gameData.shipAvaillable1=GetMergedBool(gameData.shipAvaillable1,PSParams.GameParameters.defaultAvaillabilityShip1);
		gameData.shipAvaillable2=GetMergedBool(gameData.shipAvaillable2,PSParams.GameParameters.defaultAvaillabilityShip2);
		gameData.shipAvaillable3=GetMergedBool(gameData.shipAvaillable3,PSParams.GameParameters.defaultAvaillabilityShip3);
		gameData.shipAvaillable4=GetMergedBool(gameData.shipAvaillable4,PSParams.GameParameters.defaultAvaillabilityShip4);
		gameData.shipAvaillable5=GetMergedBool(gameData.shipAvaillable5,PSParams.GameParameters.defaultAvaillabilityShip5);
		gameData.shipAvaillable6=GetMergedBool(gameData.shipAvaillable6,PSParams.GameParameters.defaultAvaillabilityShip6);
		gameData.shipAvaillable7=GetMergedBool(gameData.shipAvaillable7,PSParams.GameParameters.defaultAvaillabilityShip7);
		gameData.shipAvaillable8=GetMergedBool(gameData.shipAvaillable8,PSParams.GameParameters.defaultAvaillabilityShip8);

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
