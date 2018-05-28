using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PSPhoton;
public enum ServerInfoLabelState{CONNECTING_TO_PUN,RECONNECTING,DISCONNECTED,SERVERCONNECTED,CONNECTING_TO_LOBBY,LOBBYCONNECTED,CONNECTING_TO_ROOM,ROOMCONNECTED};
public class ServerInfoLabel : MonoBehaviour {

	#region  Public変数
	//[Header("参照用")]
	
	//[Header("Public変数")]
	public ServerInfoLabelState currentState=ServerInfoLabelState.CONNECTING_TO_PUN;
	public UILabel stateLb;
	public TweenAlpha ta;
	public string[] infoTexts;
	public LobbyManager lobbyManager;
	#endregion

	#region  メンバ変数
	
	#endregion

	#region  初期化
	void Awake () {
	
	}
	void Start () {
	
	}
	#endregion

	#region  Update
	void Update(){
		if(isAnime){
			time+=Time.deltaTime;
			if(time>twinkTime){
				time=0.0f;
				num++;
				animeString2+=".";
				if(num>5){
					num=0;
					animeString2="";
				}

				SetLabel(animeString+animeString2,false);
			}
		}
	}
	#endregion

	#region  Public関数
	public void Test(){
		SetText(ServerInfoLabelState.CONNECTING_TO_PUN);
	}
	public void Test2(){
		SetText(ServerInfoLabelState.ROOMCONNECTED);
	}
	public void SetText(ServerInfoLabelState state){
		currentState=state;
		StopAnime();
		StopBlink();
		stateLb.alpha=1f;

		switch(state){
			case ServerInfoLabelState.CONNECTING_TO_PUN:
				SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "サーバ接続中" :"Connecting Server");
				break;
			case ServerInfoLabelState.RECONNECTING:
				//数秒後に再接続
				SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "数秒後に再接続" :"Reconnecting");
				break;
			case ServerInfoLabelState.DISCONNECTED:
				SetLabel(Localization.Get("ServerInfo_Disconnected"),true);
				break;
			case ServerInfoLabelState.SERVERCONNECTED:
				SetLabel(Localization.Get("ServerInfo_LobbyDisConnected"),true);
				break;
			case ServerInfoLabelState.CONNECTING_TO_LOBBY:
				SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "ロビー接続中" :"Connecting Lobby");
				break;
			case ServerInfoLabelState.LOBBYCONNECTED:
				infoTexts=new string[3];
				infoTexts[0]=Application.systemLanguage == SystemLanguage.Japanese? "接続サーバー:"+DataManager.Instance.envData.serverRegion :
					"Server:"+DataManager.Instance.envData.serverRegion;
				infoTexts[1]=GetConUser();
				infoTexts[2]=GetRoom();
				SetLabel(infoTexts[0],false);
				StartBlink();
				break;
			case ServerInfoLabelState.CONNECTING_TO_ROOM:
				SetAnime(Application.systemLanguage == SystemLanguage.Japanese? "マッチ接続中" :"Connecting Room");
				break;
			case ServerInfoLabelState.ROOMCONNECTED:
				infoTexts=new string[3];
				infoTexts[0]=Application.systemLanguage == SystemLanguage.Japanese? "参戦受付中" :"Matching";
				infoTexts[1]=GetMinUser();
				infoTexts[2]=GetRoom();
				SetLabel(infoTexts[0],false);
				StartBlink();
				break;

		}
	}
	#endregion

	#region  ボタンなどの受け取りイベント
	public void OnBlinked(){
		if(ta.direction==AnimationOrTween.Direction.Forward){
			//次のテキスト設定
			if(blinkNum>=infoTexts.Length){
				blinkNum=0;
			}

			switch(blinkNum){
				case 0:
					if(currentState==ServerInfoLabelState.LOBBYCONNECTED){
						infoTexts[1]=GetConUser();
					}else{
						infoTexts[1]=GetMinUser();
					}
					
					break;
				case 1:
					infoTexts[2]=GetRoom();
					
					break;
			}


			SetLabel(infoTexts[blinkNum],false);
			blinkNum++;
			PlayBlink();
		}else if(ta.direction==AnimationOrTween.Direction.Reverse){
			Invoke("PlayBlinkBack",2f);
		}
	}
	#endregion

	#region  イベント

	#endregion

	#region  メンバ関数
	string GetConUser(){
		return lobbyManager.countOfPlayerOnline.ToString()+Localization.Get("ServerInfo_ConUser");
	}
	string GetRoom(){
		return lobbyManager.roomOnline.ToString()+Localization.Get("ServerInfo_Room");
	}
	string GetMinUser(){
		string str="0";

		str+=PhotonNetwork.playerList.Length.ToString()+"/"+lobbyManager.maxPlayers.ToString();

		return str;
	}
	int blinkNum=0;
	void StartBlink(){
		blinkNum=0;
		ta.ResetToBeginning();
		stateLb.alpha=1f;
		ta.enabled=true;
		ta.PlayForward();
	}

	void StopBlink(){
		CancelInvoke("PlayBlinkBack");
		ta.enabled=false;
		stateLb.alpha=1f;
	}

	void PlayBlink(){
		ta.PlayReverse();
	}
	void PlayBlinkBack(){
		ta.PlayForward();
	}
	void SetLabel(string text,bool isAbnormal){
		stateLb.color=!isAbnormal? Color.white:Color.red;
		stateLb.effectColor=!isAbnormal? Color.white:Color.red;
		stateLb.text=text;
	}
	string animeString="";
	string animeString2="";
	bool isAnime=false;
	float time=0.0f;
	public float twinkTime=0.1f;
	int num=0;
	void SetAnime(string str){
		animeString=str;
		animeString2="";
		num=1;
		time=0.0f;
		isAnime=true;
		SetLabel(str,false);
	}

	void StopAnime(){
		isAnime=false;

		animeString2="";
		num=0;
	}
	#endregion
}
