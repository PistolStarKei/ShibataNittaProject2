using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colorful;
using PSParams;
/// <summary>
/// PU_Ticketの説明
/// </summary>
public class PU_Ticket : MonoBehaviour {

	#region  メンバ変数
	public GameObject btn;
	public GameObject container;
	#endregion


	#region  Public関数
	public void Show(){
		AudioController.Play("open");
		btn.SetActive(true);
		container.SetActive(true);
		UpdateButtons();
		if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.HideBanner();
	}

	public void OnClose(){
		AudioController.Play("popup");
		btn.SetActive(false);
		container.SetActive(false);
		if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.ShowBanner();
	}

	public void OnClickMail(){
		
		AndroidSocialGate.SendMail(Application.systemLanguage == SystemLanguage.Japanese? "メール送信" :"Send Mail", "何でお困りでしょうか？", "[SBR]購入の問い合わせAndroid :"+Application.version, PSParams.AppData.MAIL);
	}

	public void OnClickTokusho(){
		Application.OpenURL(PSParams.AppData.URL_TOKUSHO);

	}
	public void OnClickPolicy(){
		Application.OpenURL(PSParams.AppData.URL_POLICY);
	}

	public void OnClickTweet(){
		if(DataManager.Instance.gameData.tweetNum>=3)return;
		PSGUI.WaitHUD.guiWait.Show(gameObject.GetComponent<UIPanel>().depth,"Connecting");	
		//ツイッターへ飛ばす、待ち受けて追加する
		//Texture2D image = GetImage();

		/*AndroidSocialGate.StartShareIntent(Application.systemLanguage == SystemLanguage.Japanese? "おすすめのゲーム" :"Cool game!"
			, PSParams.AppData.APP_TITTLE+"-> "+PSParams.AppData.APP_URL, "twi");*/
		string mes="";
		mes+=Application.systemLanguage == SystemLanguage.Japanese? "おすすめのゲーム" :"Cool game!";
		mes+=" "+PSParams.AppData.APP_TITTLE+"-> "+PSParams.AppData.APP_URL;

		PS_Plugin.Instance.twListener.Tweet(mes,OnTweetSuccessed);

		OnClose();
	}

	public void OnTweetSuccessed(bool isSuccess){
		
		PSGUI.WaitHUD.guiWait.Hide();
		if(isSuccess && DataManager.Instance.gameData.gameTickets!=-100){
			

			DataManager.Instance.gameData.tweetNum++;
			DataManager.Instance.gameData.gameTickets+=DataManager.Instance.gameData.tweetNum==3?2:1;

			DataManager.Instance.SaveAll();
			UpdateTicketNum();
			AudioController.Play("successed");
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
				SystemLanguage.Japanese? "ツイートに成功しました。" :"Post to TWITTER successed");
		}else{
			AudioController.Play("failled");
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
				SystemLanguage.Japanese? "ツイートに失敗しました。" :"Post to TWITTER failled");
		}
	}


	public void OnClickTicket5(){
		if(DataManager.Instance.gameData.gameTickets==-100)return;
		if(!PS_Plugin.Instance.storeListener.IsStoreAvaillable())return;

		PSGUI.WaitHUD.guiWait.Show(7,"Connecting");

		PS_Plugin.Instance.storeListener.PurchaseProduct(AppData.IAP_SKUs[0],OnTicket5PurchaseSuccesed,OnTicket5PurchaseFailled);
		OnClose();
	}
	public void OnTicket5PurchaseSuccesed(string mess){
		PSGUI.WaitHUD.guiWait.Hide();
		UpdateButtons();
		AudioController.Play("successed");
		PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
			SystemLanguage.Japanese? "購入が完了しました。" :"Purchase successed");
	}
	public void OnTicket5PurchaseFailled(string mess){
		if(mess=="購入完了も、消費に失敗　再起動せよ"){
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
				SystemLanguage.Japanese? "購入の反映に失敗しました、ゲームを再起動してください" :"Data affect failled.Reboot application.");
		}else{
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
				SystemLanguage.Japanese? "購入に失敗しました。" :"Purchase failled");
		}
		AudioController.Play("failled");
		PSGUI.WaitHUD.guiWait.Hide();
	}

	public void OnClickMuseigenPack(){
		if(DataManager.Instance.gameData.gameTickets==-100)return;
		if(!PS_Plugin.Instance.storeListener.IsStoreAvaillable())return;

		PSGUI.WaitHUD.guiWait.Show(7,"Connecting");
		PS_Plugin.Instance.storeListener.PurchaseProduct(AppData.IAP_SKUs[1],OnMuseigenPackPurchaseSuccesed,OnMuseigenPackPurchaseFailled);
		OnClose();
	}
	public void OnMuseigenPackPurchaseSuccesed(string mess){
		PSGUI.WaitHUD.guiWait.Hide();
		UpdateButtons();
		AudioController.Play("successed");
		AdManager.Instance.HideBanner();
		PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
			SystemLanguage.Japanese? "購入が完了しました。" :"Purchase successed");

	}
	public void OnMuseigenPackPurchaseFailled(string mess){
		if(mess=="購入完了も、消費に失敗　再起動せよ"){
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
				SystemLanguage.Japanese? "購入の反映に失敗しました、ゲームを再起動してください" :"Data affect failled.Reboot application.");
		}else{
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
				SystemLanguage.Japanese? "購入に失敗しました。" :"Purchase failled");
		}
		AudioController.Play("failled");
		PSGUI.WaitHUD.guiWait.Hide();

	}


	#endregion
	

	#region  メンバ関数

	public BuyBtn[] mBuyBtns;
	public Color btnColorActive;
	public Color btnColorActiveWaku;
	public Color btnColorNActive;
	public Color btnColorNActiveWaku;

	void UpdateButtons(){

		if(DataManager.Instance.gameData.tweetNum>=3 || DataManager.Instance.gameData.gameTickets==-100){
			mBuyBtns[0].SetPrice("N/A");
			mBuyBtns[0].SetColor(btnColorNActive,btnColorNActiveWaku);

		}else{
			
			mBuyBtns[0].SetPrice(Localization.Get("ZeroPrice"));
			mBuyBtns[0].SetColor(btnColorActive,btnColorActiveWaku);
		}

		//購入ボタン
		if(PS_Plugin.Instance.storeListener.IsStoreAvaillable()){

			if(DataManager.Instance.gameData.gameTickets==-100){
				//無制限を購入済み
				mBuyBtns[1].SetPrice("--");
				mBuyBtns[1].SetColor(btnColorNActive,btnColorNActiveWaku);
				mBuyBtns[2].SetPrice("--");
				mBuyBtns[2].SetColor(btnColorNActive,btnColorNActiveWaku);
			}else{
				//価格を設定

				mBuyBtns[1].SetPrice(PS_Plugin.Instance.storeListener.GetPrice(AppData.IAP_SKUs[0]));
				mBuyBtns[1].SetColor(btnColorActive,btnColorActiveWaku);
				mBuyBtns[2].SetPrice(PS_Plugin.Instance.storeListener.GetPrice(AppData.IAP_SKUs[1]));
				mBuyBtns[2].SetColor(btnColorActive,btnColorActiveWaku);
			}

		}else{
			mBuyBtns[1].SetPrice("--");
			mBuyBtns[1].SetColor(btnColorNActive,btnColorNActiveWaku);
			mBuyBtns[2].SetPrice("--");
			mBuyBtns[2].SetColor(btnColorNActive,btnColorNActiveWaku);
		}



	}

	#endregion

	void UpdateTicketNum(){
		GameObject go=GameObject.FindGameObjectWithTag("TicketSetter");
		if(go){
			GameTicketSetter setter=go.GetComponent<GameTicketSetter>();
			if(setter){
				setter.UpdateTickets();
			}
		}
	}
}
