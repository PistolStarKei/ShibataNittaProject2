using UnityEngine;
using System.Collections;

public class LobbyNameInput : MonoBehaviour {



	public void OnTapInput(){
		
	}

	public int nameCharacterLimit=20;
	void Start(){
		SetStartValue("UNKNOWNUSER");
		input.characterLimit=nameCharacterLimit;
	}
	public UIInput input;


	public void SetStartValue(string str){
		input.defaultText=str;
	}

	public void OnSubmitValue(){
		Debug.Log(""+input.value);
	}




}
