using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// RankingBtnの説明
/// </summary>
public class RankingBtn : MonoBehaviour {


	#region  メンバ関数
		void OnClick() {
			Debug.Log("OnClick");
			if(PS_Plugin.Instance.readerboadListener.isLogin()){
				PS_Plugin.Instance.readerboadListener.Open();
			}else{
				PSPhoton.LobbyManager.instance.info.Log(
				Application.systemLanguage == SystemLanguage.Japanese? "サーバーエラーにより取得できませんでした" :"Ranking retrieve failled");
			}
		}
	#endregion
}
