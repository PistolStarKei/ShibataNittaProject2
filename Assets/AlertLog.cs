using UnityEngine;
using System.Collections;

public class AlertLog : MonoBehaviour {

	public UILabel lb;
	public void UpdateLog(string str){
		if(!gameObject.activeSelf)NGUITools.SetActive(gameObject,true);
		lb.text=str;
	}
	public void Hide(){
		NGUITools.SetActive(gameObject,false);
	}
}
