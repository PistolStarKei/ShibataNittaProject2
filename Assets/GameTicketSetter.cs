using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PSGameUtils;
/// <summary>
/// GameTicketSetterの説明
/// </summary>
public class GameTicketSetter : MonoBehaviour {

	#region  メンバ変数
	public UILabel mTicketNumLb;
	public UILabel mTimerLb;
	public UISprite mTicketIcon;

	public GameObject mAddBtn;
	public bool isTimer=false;
	public Color fullColor;
	public Color emptyColor;
	#endregion

	#region  初期化

	void Start () {
		UpdateTickets();
	}
	#endregion


	#region  Update
	
	void Update(){
		if(isTimer){
			
			DataManager.Instance.gameData.timeForNextTickets-=Time.deltaTime;

			if(DataManager.Instance.gameData.timeForNextTickets<0.0f){
				OnAddTickets();
				CheckTickets();

			}else{
				//残りタイムの表示
				mTimerLb.text= GameUtils.FormatTime(DataManager.Instance.gameData.timeForNextTickets);
				mTimerLb.text+=Application.systemLanguage == SystemLanguage.Japanese? " 後に回復" :" b 1up";
			}
		}
	}

	#endregion


	#region  Public関数
	public void AddTickets(int num){
		DataManager.Instance.gameData.gameTickets+=num;
		DataManager.Instance.SaveAll();
		UpdateTickets();
	}
	public void AddBuyTicket(){
		DataManager.Instance.gameData.gameTickets=-100;
		DataManager.Instance.SaveAll();
		UpdateTickets();
	}
	public void OnClickAddBtn(){
		//購入画面をみせる

	}

	#endregion
	

	#region  メンバ関数

	void SetColor(bool isEmpty){
		if(!isEmpty){
			mTicketNumLb.color=fullColor;
			mTicketIcon.color=fullColor;
		}else{
			mTicketNumLb.color=emptyColor;
			mTicketIcon.color=emptyColor;
		}
	}

	void OnAddTickets(){
		//ここでチケットを追加する。
		DataManager.Instance.gameData.timeForNextTickets=-1.0f;
		DataManager.Instance.SaveAll();
		isTimer=false;
		AddTickets(1);
	
	}

	public void CheckTickets(){

		if(DataManager.Instance.gameData.gameTickets<=0 && DataManager.Instance.gameData.gameTickets!=-100){
			
			if(DataManager.Instance.gameData.timeForNextTickets<0.0f){

				DataManager.Instance.gameData.timeForNextTickets=PSParams.GameParameters.TimeForNextTicket;
				DataManager.Instance.SaveAll();
			}

			//差分を減少させる
			float keika=TimeManager.Instance.KeikaSecondSinceLast(TimeManager.StringToDateTime(DataManager.Instance.gameData.lastTime));
			DataManager.Instance.gameData.timeForNextTickets-=keika;

			Debug.Log("チケットチェック：前回からの経過時間　"+keika);
		
			isTimer=true;
			NGUITools.SetActive(mTimerLb.gameObject,true);
			SetColor(isTimer);
		}else{
			isTimer=false;

			NGUITools.SetActive(mTimerLb.gameObject,false);
			SetColor(isTimer);
		}
	}



	void SetNumText(string str){
		mTicketNumLb.text=str;
	}
	public void UpdateTickets(){
		int num=DataManager.Instance.gameData.gameTickets;
		SetNumText(num==-100? "∞":num.ToString());


		CheckTickets();

		if(num==-100){
			//無限購入済み
			NGUITools.SetActive(mAddBtn,false);
			return;
		}

	}
	#endregion
}
