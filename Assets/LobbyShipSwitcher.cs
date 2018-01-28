using UnityEngine;
using System.Collections;

public class LobbyShipSwitcher : MonoBehaviour {

	public ShipSwitcher switcher;
	public PSPhoton.LobbyManager lobby;
	public SphericalIndicator indecator;

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

	}

	public int currentSelectColor=0;
	public int currentSelect=0;
	public void OnShipChanged(int num){
		currentSelect=num;
		SetCurrentShipName(shipNames[num]);
		switcher.Set(num);
		lobby.OnShipChanged(num,currentSelectColor);
		DataManager.Instance.gameData.shipType=num;
		DataManager.Instance.SaveAll();
	}
	public UILabel currentShipNameLb;
	public void SetCurrentShipName(string name){
		currentShipNameLb.text=name;

	}





}
