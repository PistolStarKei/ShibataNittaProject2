using UnityEngine;
using System.Collections;

public class ShipSelecter : MonoBehaviour {

	public string[] shipNames;
	public ScrollItem[] items;
	[SerializeField]
	bool[] itemFlags;


	void Start(){
		itemFlags=new bool[items.Length];
		for(int i=0;i<itemFlags.Length;i++){
			items[i].itemTittle.text=shipNames[i];
			itemFlags[i]=false;
		}
		int num=DataManager.Instance.gameData.shipType;

		if(num>=itemFlags.Length){
			Debug.LogError("num==over Length");
		}else{
			currentSelect=num;
			itemFlags[num]=true;
			UpdateItems();
		}


	}

	void ClearAll(){
		for(int i=0;i<itemFlags.Length;i++){
			itemFlags[i]=false;

		}
	}

	public UILabel currentShipNameLb;
	public void SetCurrentShipName(string name){
		currentShipNameLb.text=name;

	}
	public ShipSwitcher switcher;
	public PSPhoton.LobbyManager lobby;

	void UpdateItems(){
		for(int i=0;i<itemFlags.Length;i++){
			items[i].SetState(itemFlags[i]);
			if(itemFlags[i]){
				SetCurrentShipName(shipNames[i]);
				switcher.Set(i);
				lobby.OnShipChanged(i,currentSelectColor);
				DataManager.Instance.gameData.shipType=i;
				DataManager.Instance.SaveAll();
			}
		}
	}

	public int currentSelect=0;
	public int currentSelectColor=0;
	public void OnClickItem(string name){
		ClearAll();
		currentSelect=int.Parse(name)-1;
		itemFlags[currentSelect]=true;
		UpdateItems();
	}

	public float enableAlpha=0.1f;
	public float disbleAlpha=1.0f;

}
