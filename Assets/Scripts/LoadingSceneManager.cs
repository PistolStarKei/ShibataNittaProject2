using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour {

	#region  メンバ変数
	public UISlider slider;
	public int maxProgress;
	bool isLoaded=false;
	#endregion

	#region  Public変数

	#endregion


	#region  初期化
	void Start () {
		
		PS_Plugin.Instance.InitAll(OnInitProgress);
	}
	#endregion

	#region  Public関数
	public void OnInitProgress(int progress){
		//Debug.Log(""+progress);

		slider.value=GetProgress(progress);
		if(progress==maxProgress && !isLoaded){
			isLoaded=true;
			LoadLobby();
		}
	}

	#endregion

	#region  メンバ関数
	float GetProgress(int progress){
		float num=progress/maxProgress;
		return num;
	}
	void LoadLobby(){
		SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
	}
	#endregion
}
