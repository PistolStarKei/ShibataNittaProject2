using UnityEngine;
using System.Collections;
using Colorful;

/// <summary>
/// YesNoPUの説明
/// </summary>
public class YesNoPU : MonoBehaviour {

	#region  メンバ変数
	public GaussianBlur blur;
	public UILabel label;
	public GameObject btnBG;
	public GameObject container;
	#endregion

	#region  初期化

	void Awake () {
	}

	void Start () {
	
	}
	#endregion


	#region  Update
	
	void Update(){
	
	}

	#endregion


	


	#region  Public関数
	public void Show(string desc){
		label.text=desc;
		AudioController.Play("open");
		blur.enabled=true;
		btnBG.SetActive(true);
		container.SetActive(true);
	}

	public void OnClose(){
		AudioController.Play("popup");
		blur.enabled=false;
		btnBG.SetActive(false);
		container.SetActive(false);
	}

	public void OnClickYes(){

		OnClose();
	}

	public void OnClickNo(){

		OnClose();
	}

	#endregion
	

	#region  メンバ関数
	
	#endregion
}
