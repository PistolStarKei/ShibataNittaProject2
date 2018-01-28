using UnityEngine;
using System.Collections;

public class LobbyShipSwitcher : MonoBehaviour {

	public ShipSwitcher switcher;
	public PSPhoton.LobbyManager lobby;
	public SphericalIndicator indecator;

	public ShipColorLists colorLists;

	public string[] shipNames;

	void Awake(){
		shipNames=PSParams.GameParameters.shipNames;
	}

	// Use this for initialization
	void Start () {
		indecator.onChanged+=OnShipChanged;
		currentSelect=DataManager.Instance.gameData.shipType;
		indecator.SetCurrent(currentSelect);
		switcher.Set(currentSelect);
		SetCurrentShipName(shipNames[currentSelect]);
		colorLists.SetItems(currentSelect,DataManager.Instance.gameData.shipColors[currentSelect]);

	}

	public int currentSelect=0;
	public void OnShipChanged(int num){
		currentSelect=num;
		SetCurrentShipName(shipNames[num]);
		switcher.Set(num);
		lobby.OnShipChanged(num,colorLists.mCurrentSelected);
		colorLists.SetItems(currentSelect,DataManager.Instance.gameData.shipColors[currentSelect]);
		DataManager.Instance.gameData.shipType=num;
		DataManager.Instance.SaveAll();
	}
	public UILabel currentShipNameLb;
	public void SetCurrentShipName(string name){
		currentShipNameLb.text=name;

	}

	public UILabel currentShipSubNameLb;
	public void SetCurrentShipSubName(string name){
		currentShipSubNameLb.text=name;

	}



}
