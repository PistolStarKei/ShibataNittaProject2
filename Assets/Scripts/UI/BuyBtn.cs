using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BuyBtnの説明
/// </summary>
public class BuyBtn : MonoBehaviour {

	#region  メンバ変数
	public UILabel mPriceLb;
	public UISprite mBgSp;
	public UISprite mBgWakuSp;
	#endregion

	#region  Public関数
	public void SetPrice(string str){
		mPriceLb.text=str;
	}
	public void SetColor(Color bgColor,Color wakuColor){
		mBgSp.color=bgColor;
		mBgWakuSp.color=wakuColor;

	}

	#endregion

}
