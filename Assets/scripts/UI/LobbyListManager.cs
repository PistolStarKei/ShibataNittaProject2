using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LobbyListManager : MonoBehaviour {

	public List<RoomUserList> userList=new List<RoomUserList>();

	public GameObject listPrehab;
	public UIGrid grid;


	public UISprite bg;
	void CheckVisible(){
		if(userList.Count<=0){
			bg.enabled=false;
		}else{
			bg.enabled=true;
		}	
	}

	public void ClearList(){
		foreach(RoomUserList list in userList){
			list.Destroy();
		}
		userList.Clear();
		CheckVisible();
	}

	public void AddList(string userName,int num,string countly){
		
		GameObject go=Instantiate(listPrehab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity) as GameObject;
		go.name=num.ToString();
		go.transform.parent=grid.transform;
		go.transform.localPosition=Vector3.zero;
		go.transform.localRotation=Quaternion.identity;
		go.transform.localScale= new Vector3(0.7195626f, 0.7195626f, 0.7195626f);

		RoomUserList list=go.GetComponent<RoomUserList>();
		list.SetUserName(userName);
		userList.Add(list);
		grid.Reposition();
		CheckVisible();
	}
		



}
