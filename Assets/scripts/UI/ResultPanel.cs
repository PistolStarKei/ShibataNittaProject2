using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultPanel : MonoBehaviour {


	public PS_GUI_Cover cover;

	public void ShowResult(float time,int killNum,List<shipControl> deadShips,List<shipControl> ships,shipControl playerShip){
		isShowing=true;

		//全参加者数
		int players=ships.Count;

		//プレイヤの順位
		string playerRank=GetRank(deadShips,ships,playerShip)+"/"+players.ToString();
		//プレイヤの生存時間
		int minutes = Mathf.FloorToInt(time / 60F);
		int seconds = Mathf.FloorToInt(time - minutes * 60);
		string aliveTime= string.Format("{0:00}:{1:00}", minutes, seconds);
		//プレイヤのキル数
		string kills=killNum.ToString();

		SetUserData(playerShip.playerData.countlyCode,playerShip.playerData.userName,aliveTime,playerRank,kills);

		Invoke("Show",3.0f);
	}

	public void Show(){
		ta_panel.PlayForward();
		cover.CoverWithBlackMask();
	}

	public UILabel userName;
	public UILabel userTime;
	public UILabel userRank;
	public UILabel userKill;
	public UISprite userFlag;
	void SetUserData(string flag,string name,string time,string rank,string kill){
		userName.text=name;
		userTime.text=time;
		userRank.text=rank;
		userKill.text=kill;
		userFlag.spriteName=flag;
	}


	string GetRank(List<shipControl> deadShips,List<shipControl> ships,shipControl playerShip){
		//参加者数
		int i=ships.Count;

		foreach(shipControl ship in deadShips){
			if(ship==playerShip)return i.ToString();
			i--;
		}

		return "";
	}



	public void CloseResult(){
		ta_panel.PlayReverse();
	}

	public void OnCliclMain(){
		PSPhoton.GameManager.instance.BackToMain();
	}


	public bool isShowing=false;

	public TweenAlpha ta_panel;
	public void OnTweenPanel(){
		if(ta_panel.direction==AnimationOrTween.Direction.Forward){
			NGUITools.SetActive(backButton,true);
		}else if(ta_panel.direction==AnimationOrTween.Direction.Reverse){
			isShowing=false;
		}
	}

	public GameObject backButton;

}
