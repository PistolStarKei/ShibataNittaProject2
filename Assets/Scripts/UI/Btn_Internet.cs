﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Btn_Internetの説明
/// </summary>
public class Btn_Internet : MonoBehaviour {

	#region  メンバ変数
	public PU_HowTo howto;
	#endregion

	#region  初期化

	void Awake () {
	}

	void Start () {
	
	}
	#endregion


	#region  Update
	
	void Update(){
	
	}

	#endregion


	


	#region  Public関数
	public void OnClick(){
		howto.Show();
	}

	public void OnResponse(bool isYes){
		if(isYes){
			DataManager.Instance.gameData.isReviewd=true;
			DataManager.Instance.SaveAll();
			Application.OpenURL(PSParams.AppData.APP_URL);
		}
	}
	#endregion
	

	#region  メンバ関数
	
	#endregion
}
