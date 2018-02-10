using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colorful;
using PSParams;

/// <summary>
/// PU_Infoの説明
/// </summary>
public class PU_Info : MonoBehaviour {

	#region  メンバ変数
	public GameObject btn;
	public GameObject container;
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
	public void Show(){
		AudioController.Play("open");
		btn.SetActive(true);
		container.SetActive(true);
	}

	public void OnClose(){
		AudioController.Play("popup");
		btn.SetActive(false);
		container.SetActive(false);
	}

	public void OnClickMail(){

		AndroidSocialGate.SendMail(Application.systemLanguage == SystemLanguage.Japanese? "メール送信" :"Send Mail", "何でお困りでしょうか？", "[SBR]不具合報告", PSParams.AppData.MAIL);

	}

	public void OnClickTwitter(){
		if(!PS_Plugin.Instance.twListener.IsAuthenticated)return;


		PSGUI.WaitHUD.guiWait.Show(gameObject.GetComponent<UIPanel>().depth,"Connecting");	
		//ツイッターへ飛ばす、待ち受けて追加する
		PS_Plugin.Instance.twListener.Follow(FollowComplete);
	}

	public void FollowComplete(bool success){
		PSGUI.WaitHUD.guiWait.Hide();
	}
	public void OnClickPoricy(){
		Application.OpenURL(PSParams.AppData.URL_POLICY);
	}


	#endregion
	

	#region  メンバ関数
	
	#endregion
}
