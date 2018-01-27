using UnityEngine;
using System.Collections;

public enum NetworkState{DISCONNECTED,SERVERCONNECTED,LOBBYCONNECTED,ROOMCONNECTED};
public class LobbyStateHUD : MonoBehaviour {

	public NetworkState networkState=NetworkState.DISCONNECTED;

	public UILabel stateLb;
	public void SetLabel(string text,bool isAbnormal){
		stateLb.color=!isAbnormal? normalC:abnormalC;
		stateLb.effectColor=!isAbnormal? normalC:abnormalC;
		stateLb.text=text;
	}
	public UISprite state1;
	public UISprite state2;
	public UISprite state3;


	public Color normalC;
	public Color abnormalC;

	public GameObject PlayBtn;
	public TweenRotation PlayBtnRot;
	public GameObject TimerBtn;

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
		SetLabel(str,false);
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
				if(num>5){
					num=0;
					animeString2="";
				}
				SetLabel(animeString2,false);
			}
		}
	}


	public void SetStateHUD(NetworkState networkState){
		this.networkState=networkState;
		StopAnime();
		switch(networkState){
			case NetworkState.DISCONNECTED:
			
				state1.color=normalC;
				state2.color=abnormalC;
				state3.color=abnormalC;
				PlayBtnRot.enabled=false;
				PlayBtnRot.enabled=false;
				NGUITools.SetActive(PlayBtn,true);
				NGUITools.SetActive(TimerBtn,false);
				SetLabel("ERROR",true);
				break;
			case NetworkState.SERVERCONNECTED:
				state1.color=normalC;
				state2.color=normalC;
				state3.color=abnormalC;
				PlayBtnRot.enabled=false;
				NGUITools.SetActive(PlayBtn,true);
				NGUITools.SetActive(TimerBtn,false);
				SetLabel("ERROR",true);
				break;
			case NetworkState.LOBBYCONNECTED:
				state1.color=normalC;
				state2.color=normalC;
				state3.color=normalC;
				PlayBtnRot.enabled=true;
				NGUITools.SetActive(PlayBtn,true);
				NGUITools.SetActive(TimerBtn,false);
				SetLabel("READY",false);
				break;
			case NetworkState.ROOMCONNECTED:
				state1.color=normalC;
				state2.color=normalC;
				state3.color=normalC;
				PlayBtnRot.enabled=false;
				NGUITools.SetActive(PlayBtn,false);
				NGUITools.SetActive(TimerBtn,true);
				SetLabel(Application.systemLanguage == SystemLanguage.Japanese? "参戦受付中" :"Matching",false);
				break;
		}

	}
}
