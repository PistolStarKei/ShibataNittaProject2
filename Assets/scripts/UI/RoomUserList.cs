using UnityEngine;
using System.Collections;

public class RoomUserList : MonoBehaviour {

	public UILabel userName;

	public void SetUserName(string name){
		userName.text=name;
	}

	public void Destroy(){
		Destroy(gameObject);
	}
}
