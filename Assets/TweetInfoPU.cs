using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweetInfoPU : MonoBehaviour {

	public GameObject btnBG;
	public GameObject container;

	#region  Public関数
	public void Show(){
		AudioController.Play("open");
		btnBG.SetActive(true);
		container.SetActive(true);
	}

	public void OnClose(){
		AudioController.Play("popup");
		btnBG.SetActive(false);
		container.SetActive(false);
	}

	public void OnClickClose(){
		OnClose();
	}

	#endregion


	#region  メンバ関数

	#endregion
}
