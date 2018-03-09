using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour {

	#region  メンバ変数
	
	#endregion

	#region  Public変数
	
	#endregion


	#region  初期化
	void Start () {
		PS_Plugin.Instance.InitAll(OnInitProgress);
	}
	#endregion

	#region  Update
	// Update is called once per frame
	void Update () {
	
	}
	#endregion

	#region  Public関数
	public void OnInitProgress(int progress){
		Debug.Log(""+progress);

	}

	#endregion

	#region  メンバ関数
	void LoadLobby(){
		SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
	}
	#endregion
}
