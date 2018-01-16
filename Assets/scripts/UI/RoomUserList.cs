using UnityEngine;
using System.Collections;

public class RoomUserList : MonoBehaviour {

	public UILabel userName;

	public UISprite sp;
	public void SetUserName(string name){
		userName.text=name;
	}

	public void SetUserCountly(string name){
		sp.spriteName=name;	
	}

	public void Destroy(){
		Destroy(gameObject);
	}
}
