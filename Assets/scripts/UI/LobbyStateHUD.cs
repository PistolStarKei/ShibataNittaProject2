using UnityEngine;
using System.Collections;

public enum NetworkState{DISCONNECTED,SERVERCONNECTED,LOBBYCONNECTED,ROOMCONNECTED};
public class LobbyStateHUD : MonoBehaviour {

	public NetworkState networkState=NetworkState.DISCONNECTED;

	public UILabel stateLB;
	public UISprite state1;
	public UISprite state2;
	public UISprite state3;

	public UISprite connect1;
	public UISprite connect2;

	public Color normalC;
	public Color abnormalC;


	public TweenRotation PlayBtnRot;

	string animeString="";
	string animeString2="";
	bool isAnime=false;
	float time=0.0f;
	public float twinkTime=0.5f;
	int num=0;
	public void SetAnime(string str){
		animeString=str;
		animeString2="";
		num=0;
		time=0.0f;
		isAnime=true;
	}

	public void StopAnime(){
		isAnime=false;

		animeString2="";
		num=0;
	}

	void Update(){
		if(isAnime){
			time=Time.deltaTime;
			if(time>twinkTime){
				time=0.0f;
				num++;
				animeString2+=".";
				stateLB.text=animeString+animeString2;
				if(num>5){
					num=0;
					animeString2="";
				}
			}
		}
	}


	public void SetStateHUD(NetworkState networkState){
		this.networkState=networkState;
		StopAnime();
		switch(networkState){
			case NetworkState.DISCONNECTED:
			
				stateLB.text= Application.systemLanguage == SystemLanguage.Japanese? "サーバー未接続" :"Disconnected";
				stateLB.color=abnormalC;
				state1.color=normalC;
				state2.color=abnormalC;
				state3.color=abnormalC;
				connect1.enabled=false;
				connect2.enabled=false;
				PlayBtnRot.enabled=false;
				break;
			case NetworkState.SERVERCONNECTED:
				stateLB.text=Application.systemLanguage == SystemLanguage.Japanese? "サーバー接続済" :"Server Connected";
				stateLB.color=abnormalC;
				state1.color=normalC;
				state2.color=normalC;
				state3.color=abnormalC;
				connect1.enabled=true;
				connect2.enabled=false;
				PlayBtnRot.enabled=false;
				break;
			case NetworkState.LOBBYCONNECTED:
				stateLB.text=Application.systemLanguage == SystemLanguage.Japanese? "ロビー接続済" :"Lobby Connected";
				stateLB.color=normalC;
				state1.color=normalC;
				state2.color=normalC;
				state3.color=normalC;
				connect1.enabled=true;
				connect2.enabled=true;
				PlayBtnRot.enabled=true;
				break;
			case NetworkState.ROOMCONNECTED:
				stateLB.text=Application.systemLanguage == SystemLanguage.Japanese? "対戦受付中" :"Matching Fight";
				stateLB.color=normalC;
				state1.color=normalC;
				state2.color=normalC;
				state3.color=normalC;
				connect1.enabled=true;
				connect2.enabled=true;
				PlayBtnRot.enabled=false;
				break;
		}

	}
}
