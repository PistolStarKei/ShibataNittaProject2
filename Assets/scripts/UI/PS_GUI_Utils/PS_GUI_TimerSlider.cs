using UnityEngine;
using System.Collections;

public class PS_GUI_TimerSlider : MonoBehaviour {

	public UISprite sp;
	public UILabel lb;

	//秒数で与える
	public void SetTime(float lastTime,float maxTime){

		int minutes = Mathf.FloorToInt(lastTime / 60F);
		int seconds = Mathf.FloorToInt(lastTime - minutes * 60);
		string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
		lb.text=niceTime;

		sp.fillAmount=lastTime/maxTime;
	}




}
