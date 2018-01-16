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
	public string country ="";
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
		ES2.Delete(filename+"?tag=country");

		if(!ES2.Exists(filename+"?tag=country")){
			
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
	}

	public void SaveAll(){
		ES2.Save(envData.toggleSE,filename+"?tag=toggleSE");
		ES2.Save(envData.toggleBGM,filename+"?tag=toggleBGM");

		ES2.Save(gameData.shipType,filename+"?tag=shipType");
		ES2.Save(gameData.username,filename+"?tag=username");
		ES2.Save(gameData.isConnectingRoom,filename+"?tag=isConnectingRoom");
		ES2.Save(gameData.country,filename+"?tag=country");

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
	}
		
	private void DeleteAll(){
		#if UNITY_EDITOR
			ES2.DeleteDefaultFolder();
		#endif
	}
}
