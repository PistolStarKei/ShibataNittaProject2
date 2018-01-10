using UnityEngine;
using System.Collections;

public class FlagSetter : MonoBehaviour {


	public UISprite sp;
	// Use this for initialization
	void Start () {
		sp.spriteName=Countly.ToCountrySPrite(DataManager.Instance.gameData.country);	
	}

}
