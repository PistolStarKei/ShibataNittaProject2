using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnlockLabel : MonoBehaviour {

	#region  Public変数
	public UILabel lb;
	public delegate void Callback_OnTimeOut();
	public  event Callback_OnTimeOut onTimeoutEvent;
	#endregion

	#region  メンバ変数
	bool mIsTimer=false;
	float mLastTime=0f;
	string formatUnlockAt="";
	#endregion

	#region  Update
	void Update () {
		if(mIsTimer){
			mLastTime-=Time.deltaTime;
			if(mLastTime>0){
				SetTime(mLastTime);
			}else{
				OnTimeOut();
			}
		}
	}
	#endregion

	#region  Public関数
	public void SetLastTime(float lastTime,Callback_OnTimeOut onTimeoutEvent){
		mLastTime=lastTime;
		this.onTimeoutEvent=onTimeoutEvent;
		SetTime(mLastTime);
		lb.enabled=true;
		mIsTimer=true;
	}
	public void SetLastTime(){
		mIsTimer=false;
		lb.enabled=false;
	}
	#endregion

	#region  ボタンなどの受け取りイベント

	#endregion

	#region  イベント
	void OnTimeOut(){
		if(this.onTimeoutEvent!=null)this.onTimeoutEvent();
		SetLastTime();
	}
	#endregion

	#region  メンバ関数
	void SetTime(float lastTime){
		if(formatUnlockAt=="")formatUnlockAt=Localization.Get("LastTimeUnlock");;

		int hours = (int)(lastTime / 3600);
		int minutes = (int)((lastTime % 3600) / 60);
		int seconds = (int)((lastTime % 3600 ) % 60);

		string niceTime = "";
		if(hours>0)niceTime+=hours.ToString()+":";
		if(hours>0){
			niceTime+=minutes.ToString()+":";
		}else{
			if(minutes>0){
				niceTime+=minutes.ToString()+":";
			}
		}

		niceTime+=seconds.ToString();

		lb.text=formatUnlockAt+niceTime;

	}
	#endregion
}
