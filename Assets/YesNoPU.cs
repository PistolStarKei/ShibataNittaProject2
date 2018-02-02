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

	public  event Callback_userResponce onResponce;
	public delegate void Callback_userResponce(bool isYes);
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
	public void Show(string desc,Callback_userResponce onResponce){
		
		this.onResponce= onResponce;
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
		if(this.onResponce!=null)this.onResponce(true);
		this.onResponce=null;
		OnClose();
	}

	public void OnClickNo(){
		if(this.onResponce!=null)this.onResponce(false);
		this.onResponce=null;
		OnClose();
	}

	#endregion
	

	#region  メンバ関数
	
	#endregion
}
