using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ShipStatus{BOADING,SELECTABLE,PURCHASABLE,PENDINNG};
/// <summary>
/// Shipへの搭乗　課金など
/// </summary>
public class BoadingBtn : MonoBehaviour {

	#region  メンバ変数
	public UISprite bg1;
	public UISprite bg2;
	public UILabel lb;
	ShipStatus mStatus;

	public  Color mBGColorBOADING;
	public  Color mBGColor2BOADING;
	public  Color mBGColorSELECTABLE;
	public  Color mBGColor2SELECTABLE;
	public  Color mBGColorPURCHASABLE;
	public  Color mBGColor2PURCHASABLE;
	public  Color mBGColorPENDING;
	public  Color mBGColor2PENDING;
	public LobbyShipSwitcher switcher;
	#endregion


	#region  Public関数
	public void SetState(ShipStatus status,string lableString){
		mStatus=status;
		switch(mStatus){
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
				lb.text=lableString;
				break;
			case ShipStatus.PENDINNG:
				bg1.color=mBGColorPENDING;
				bg2.color=mBGColor2PENDING;
				lb.text=Application.systemLanguage == SystemLanguage.Japanese? "未開放" :"Locked";
				break;
		}
	}

	#endregion
	

	#region  メンバ関数
	void OnClick(){
		Debug.Log("OnClick");
		switcher.OnClickBoadningBtn(mStatus);
	}
	#endregion
}
