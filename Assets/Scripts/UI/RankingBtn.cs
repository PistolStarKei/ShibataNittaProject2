using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// RankingBtnの説明
/// </summary>
public class RankingBtn : MonoBehaviour {

	public Ranking_PU rankingMenu;
	#region  メンバ関数
		void OnClick() {
			rankingMenu.Show();
			return;
			Debug.Log("OnClick");
			if(PS_Plugin.Instance.readerboadListener.isLogin()){
				//PS_Plugin.Instance.readerboadListener.Open();
				rankingMenu.Show();
			}else{
				PSPhoton.LobbyManager.instance.info.Log(
				Application.systemLanguage == SystemLanguage.Japanese? "サーバーエラーにより取得できませんでした" :"Ranking retrieve failled");
			}
		}
	#endregion
}
