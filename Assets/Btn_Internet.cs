﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Btn_Internetの説明
/// </summary>
public class Btn_Internet : MonoBehaviour {

	#region  メンバ変数
	public YesNoPU yesno;
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
		yesno.Show(Localization.Get("TittleReview"),OnResponse);
	}

	public void OnResponse(bool isYes){
		if(isYes){
			Application.OpenURL(PSParams.AppData.APP_URL);
		}
	}
	#endregion
	

	#region  メンバ関数
	
	#endregion
}
