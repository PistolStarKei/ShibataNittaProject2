using UnityEngine;
using System.Collections;

public enum NetworkState{DISCONNECTED,SERVERCONNECTED,LOBBYCONNECTED,ROOMCONNECTED};
public class LobbyStateHUD : MonoBehaviour {

	public NetworkState networkState=NetworkState.DISCONNECTED;

	public ServerInfoLabel _stateInfo;


	public UISprite state1;
	public UISprite state2;
	public UISprite state3;

	public GameObject WaitingTime;
	public Color normalC;
	public Color abnormalC;

	public GameObject PlayBtn;
	public TweenRotation PlayBtnRot;
	public GameObject TimerBtn;

	public void SetServerState(ServerInfoLabelState currentState){
		switch(currentState){
		case  ServerInfoLabelState.DISCONNECTED:
			state1.color=normalC;
			state2.color=abnormalC;
			state3.color=abnormalC;
			PlayBtnRot.enabled=false;
			PlayBtnRot.enabled=false;
			ShowWaitingTime(false);
			NGUITools.SetActive(PlayBtn,true);
			NGUITools.SetActive(TimerBtn,false);
			break;
		case  ServerInfoLabelState.RECONNECTING:
			state1.color=normalC;
			state2.color=abnormalC;
			state3.color=abnormalC;
			PlayBtnRot.enabled=false;
			PlayBtnRot.enabled=false;
			ShowWaitingTime(false);
			NGUITools.SetActive(PlayBtn,true);
			NGUITools.SetActive(TimerBtn,false);
			break;
		case  ServerInfoLabelState.SERVERCONNECTED:
			state1.color=normalC;
			state2.color=normalC;
			state3.color=abnormalC;
			PlayBtnRot.enabled=false;
			ShowWaitingTime(false);
			NGUITools.SetActive(PlayBtn,true);
			NGUITools.SetActive(TimerBtn,false);
			break;
		case  ServerInfoLabelState.LOBBYCONNECTED:
			state1.color=normalC;
			state2.color=normalC;
			state3.color=normalC;
			PlayBtnRot.enabled=true;
			ShowWaitingTime(false);
			NGUITools.SetActive(PlayBtn,true);
			NGUITools.SetActive(TimerBtn,false);
			break;
		case  ServerInfoLabelState.ROOMCONNECTED:
			state1.color=normalC;
			state2.color=normalC;
			state3.color=normalC;
			PlayBtnRot.enabled=false;
			ShowWaitingTime(true);
			NGUITools.SetActive(PlayBtn,false);
			NGUITools.SetActive(TimerBtn,true);
			break;
		}


		_stateInfo.SetText(currentState);
	}

	public void ShowWaitingTime(bool shoulShow){
		NGUITools.SetActive(WaitingTime,shoulShow);
	}

}
