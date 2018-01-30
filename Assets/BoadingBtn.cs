using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ShipStatus{BOADING,SELECTABLE,PURCHASABLE};
/// <summary>
/// Shipへの搭乗　課金など
/// </summary>
public class BoadingBtn : MonoBehaviour {

	#region  メンバ変数
	public UISprite bg1;
	public UISprite bg2;
	public UILabel lb;

	public  Color mBGColorBOADING;
	public  Color mBGColor2BOADING;
	public  Color mBGColorSELECTABLE;
	public  Color mBGColor2SELECTABLE;
	public  Color mBGColorPURCHASABLE;
	public  Color mBGColor2PURCHASABLE;

	public LobbyShipSwitcher switcher;
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
	public void SetState(ShipStatus status,string lableString){
		switch(status){
			case ShipStatus.BOADING:
				bg1.color=mBGColorBOADING;
				bg2.color=mBGColor2BOADING;
				lb.text=Application.systemLanguage == SystemLanguage.Japanese? "搭乗機" :"On Boad";
				break;
			case ShipStatus.SELECTABLE:
				bg1.color=mBGColorSELECTABLE;
				bg2.color=mBGColor2SELECTABLE;
				lb.text=Application.systemLanguage == SystemLanguage.Japanese? "搭乗可能" :"Boad";
				break;
			case ShipStatus.PURCHASABLE:
				bg1.color=mBGColorPURCHASABLE;
				bg2.color=mBGColor2PURCHASABLE;
				lb.text=Application.systemLanguage == SystemLanguage.Japanese? "¥" :"＄"+lableString;
				break;
		}
	}

	#endregion
	

	#region  メンバ関数
	void OnClick(){
		Debug.Log("OnClick");
		switcher.OnClickBoadningBtn();
	}
	#endregion
}
