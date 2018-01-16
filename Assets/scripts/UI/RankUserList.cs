using UnityEngine;
using System.Collections;

public class RankUserList : MonoBehaviour {

	public UILabel userName;
	public UILabel userRank;

	public UISprite sp;
	public void SetUserName(string name){
		userName.text=name;
	}

	public void SetUserCountly(string name){
		sp.spriteName=name;	
	}

	public void SetUserRank(string name){
		userRank.text=name;
	}

	public void Destroy(){
		Destroy(gameObject);
	}
}
