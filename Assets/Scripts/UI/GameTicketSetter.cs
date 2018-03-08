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

	public GameObject mAddBtn;
	public bool isTimer=false;
	public Color fullColor;
	public Color emptyColor;

	public UISprite[] ticketsObj;
	public ParticleSystem usingTickets;
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

	public void MinusTicketsNoSave(){
		if(DataManager.Instance.gameData.gameTickets!=-100){
			
			if(DataManager.Instance.gameData.gameTickets<=5){
				usingTickets.transform.position=ticketsObj[DataManager.Instance.gameData.gameTickets-1].transform.position;
				for(int i=0;i<ticketsObj.Length;i++){
					ticketsObj[i].color=fullColor;
					ticketsObj[i].enabled=false;
				}
				for(int i=0;i<ticketsObj.Length;i++){
					if(i<(DataManager.Instance.gameData.gameTickets-1)){
						ticketsObj[i].enabled=true;
					}
				}

			}else{
				usingTickets.transform.position=ticketsObj[1].transform.position;

				if(DataManager.Instance.gameData.gameTickets==6){
						for(int i=0;i<ticketsObj.Length;i++){
							ticketsObj[i].color=fullColor;
							ticketsObj[i].enabled=false;
						}
						for(int i=0;i<ticketsObj.Length;i++){
								ticketsObj[i].enabled=true;
						}
						SetNumText("");
				}
				SetNumText((DataManager.Instance.gameData.gameTickets-1).ToString());
			}

		}
		usingTickets.Play();
	}

	#endregion
	

	#region  メンバ関数


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
		}else{
			isTimer=false;

			NGUITools.SetActive(mTimerLb.gameObject,false);
		}
	}



	void SetNumText(string str){
		mTicketNumLb.text=str;
	}
	public void UpdateTickets(){
		SetTickets();
		CheckTickets();

	}

	void SetTickets(){
		int num=DataManager.Instance.gameData.gameTickets;
		if(num==-100){
			for(int i=0;i<ticketsObj.Length;i++){
				ticketsObj[i].enabled=false;
				ticketsObj[i].color=fullColor;
			}
			ticketsObj[1].enabled=true;
			SetNumText("∞");

		}else{
			if(num<=5){
				if(num<=0){
					for(int i=0;i<ticketsObj.Length;i++){
						ticketsObj[i].color=emptyColor;
						ticketsObj[i].enabled=false;
					}
					ticketsObj[0].enabled=true;
				}else{
					for(int i=0;i<ticketsObj.Length;i++){
						ticketsObj[i].color=fullColor;
						ticketsObj[i].enabled=false;
					}
					for(int i=0;i<ticketsObj.Length;i++){
						if(i<num){
							ticketsObj[i].enabled=true;
						}
					}
					SetNumText("");
				}
			}else{
				for(int i=0;i<ticketsObj.Length;i++){
					ticketsObj[i].enabled=false;
					ticketsObj[i].color=fullColor;
				}
				ticketsObj[1].enabled=true;
				SetNumText(num>99?"99":num.ToString());
			}
		}

	}
	#endregion
}
