using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PU_HowTo : MonoBehaviour {

	#region  Public変数
	public GameObject btnBG;
	public GameObject container;
	public PageIndecaters pageIndecater;
	public GameObject[] contents;
	#endregion

	#region  メンバ変数
	
	#endregion

	#region  初期化
	void Awake () {
		pageIndecater.pageNum=contents.Length;
		pageIndecater.onChanged+=OnPage;

	}
	#endregion

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


	#endregion

	#region  ボタンなどの受け取りイベント
	public void OnPage(int num){
		SetContents(num);
	}
	public void OnClickClose(){
		OnClose();
	}
	#endregion

	#region  メンバ関数
	void SetContents(int num){
		for(int i=0;i<contents.Length;i++){
			contents[i].SetActive(false);
		}
		if(num<contents.Length){
			contents[num].SetActive(true);
		}else{
			Debug.LogError("num is over length");
		}


	}
	#endregion
}
