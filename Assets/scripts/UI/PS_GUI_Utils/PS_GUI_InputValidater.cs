using UnityEngine;
using System.Collections;

public class PS_GUI_InputValidater : MonoBehaviour {

	public UIInput input;

	void Awake(){
		input=gameObject.GetComponent<UIInput>();
	}

	void Start(){
		input.defaultText=DataManager.Instance.gameData.username;
	}

	public void OnSubmit(){
		Debug.Log("OnSubmit");
		string str=input.value;

		if(str==""){
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == SystemLanguage.Japanese?"何かを入力してください":"User name could not be empty");
			SetValue(DataManager.Instance.gameData.username);
		}else{
			Debug.Log("name saved");
			DataManager.Instance.gameData.username=input.value;
			PSPhoton.LobbyManager.instance.OnUserNameChanged(input.value);
			DataManager.Instance.SaveAll();
		}

	}

	public void SetValue(string str){
		Debug.Log("SetValue "+str);

		input.value=str;
	}
}
