using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// RankingDataHolderの説明
/// </summary>
public class RankingDataHolder : MonoBehaviour {

	#region  メンバ変数
	public UILabel rankLb;
	public UILabel scoreLb;
	public UILabel nameLb;
	public UITexture iconTex;
	#endregion

	#region  Public関数
	public void SetUser(string rank,Color rankColor,string score,Texture2D icon,string name){
		rankLb.text=rank;
		rankLb.color=rankColor;
		rankLb.effectColor=rankColor;
		scoreLb.text=score;
		nameLb.text=name;
		iconTex.mainTexture=icon;
	}

	public void KillSelf(){
		Destroy(gameObject);
	}
	#endregion
	

	#region  メンバ関数
	
	#endregion
}
