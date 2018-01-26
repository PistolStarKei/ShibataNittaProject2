using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnvData{
	public bool toggleSE=true;
	public bool toggleBGM=true;
}

[System.Serializable]
public class GameData{
	public int shipType =0;
	public string username ="UNKNOWN";
	public bool isConnectingRoom =false;
	public string lastRoomName ="";
	public string country ="";
	public string lastTime ="";
	public int gameTickets =0;
	public float timeForNextTickets =0.0f;
	public string userID ="";
}

public class DataManager : PS_SingletonBehaviour<DataManager> {

	public bool isPersistantBetweenScenes=false;
	public const string filename = "SavedData.txt";

	public EnvData envData=new EnvData();
	public GameData gameData=new GameData();

	void Awake(){
		
		if(isPersistantBetweenScenes){

			DontDestroyOnLoad(this.gameObject);

			if(this != Instance)
			{
				DestroyAll();
				return;
			}

		}

		if(!ES2.Exists(filename+"?tag=lastRoomName")){
			
			InitData();
			LoadData();
		}else{
			LoadData();
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




	}

	public void SaveAll(){
		ES2.Save(envData.toggleSE,filename+"?tag=toggleSE");
		ES2.Save(envData.toggleBGM,filename+"?tag=toggleBGM");

		ES2.Save(gameData.shipType,filename+"?tag=shipType");
		ES2.Save(gameData.username,filename+"?tag=username");
		ES2.Save(gameData.isConnectingRoom,filename+"?tag=isConnectingRoom");
		ES2.Save(gameData.country,filename+"?tag=country");
		ES2.Save(gameData.lastTime,filename+"?tag=lastTime");

		ES2.Save(gameData.gameTickets,filename+"?tag=gameTickets");
		ES2.Save(gameData.timeForNextTickets,filename+"?tag=timeForNextTickets");
		ES2.Save(gameData.userID,filename+"?tag=userID");
		ES2.Save(gameData.lastRoomName,filename+"?tag=lastRoomName");
	}

	private void DestroyAll(){
		foreach (Transform childTransform in gameObject.transform) Destroy(childTransform.gameObject);
		Destroy(gameObject);
	}

	private void InitData(){
		ES2.Save(true,filename+"?tag=toggleSE");
		ES2.Save(true,filename+"?tag=toggleBGM");

		ES2.Save(0,filename+"?tag=shipType");
		ES2.Save(Application.systemLanguage == SystemLanguage.Japanese?"名無しさん":"Unkown",filename+"?tag=username");
		ES2.Save(false,filename+"?tag=isConnectingRoom");

		ES2.Save(Countly.ToCountryCode(Application.systemLanguage),filename+"?tag=country");
		ES2.Save(TimeManager.GetCurrentTime(),filename+"?tag=lastTime");
		ES2.Save(PSParams.GameParameters.DefaultTicketsNum,filename+"?tag=gameTickets");
		ES2.Save(-1.0f,filename+"?tag=timeForNextTickets");
		ES2.Save(GetUUID (),filename+"?tag=userID");
		ES2.Save("",filename+"?tag=lastRoomName");
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
}
