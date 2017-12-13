using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnvData{
	public float volume=0.5f;
	public string userName="UNKNOWN";
}

public class DataManager : PS_SingletonBehaviour<DataManager> {

	public bool isPersistantBetweenScenes=false;
	public const string filename = "SavedData.txt";

	public EnvData envData=new EnvData();

	void Awake(){
		if(isPersistantBetweenScenes){

			DontDestroyOnLoad(this.gameObject);

			if(this != Instance)
			{
				DestroyAll();
				return;
			}

		}

		if(!ES2.Exists(filename+"?tag=volume")){
			InitData();
		}else{
			LoadData();
		}

	}
	public void LoadData(){
		envData.volume=ES2.Load<float>(filename+"?tag=volume");
		envData.userName=ES2.Load<string>(filename+"?tag=userName");
	}

	private void DestroyAll(){
		foreach (Transform childTransform in gameObject.transform) Destroy(childTransform.gameObject);
		Destroy(gameObject);
	}

	private void InitData(){
		ES2.Save(0.5f,filename+"?tag=volume");
		ES2.Save("UNKNOWN",filename+"?tag=userName");
	}
		
	private void DeleteAll(){
		#if UNITY_EDITOR
			ES2.DeleteDefaultFolder();
		#endif
	}
}
