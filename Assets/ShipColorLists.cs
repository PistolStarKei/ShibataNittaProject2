using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipColorLists : MonoBehaviour {

	#region  変数
	public GameObject listItem;
	public UIGrid grid;
	public LobbyShipSwitcher switcher;
	public PSPhoton.LobbyManager lobby;
	public UIScrollView scroll;

	public List<ShipColorScrollItem> mItemLists=new List<ShipColorScrollItem>();
	public int mCurrentSelected=0;
	public string[] mShipSubNames;

	#endregion

	#region  初期化

	void Awake () {
		ShipColorScrollItem si;
		int num=0;
		foreach(Transform tr in grid.transform){
			si=tr.gameObject.GetComponent<ShipColorScrollItem>();
			if(si!=null){
				si.gameObject.name=num.ToString();
				si.shipColorLists=this;
			}
			num++;
		}
		grid.Reposition();
	}


	// Use this for initialization
	void Start () {
	
	}
	#endregion




	#region  メンバ関数

	void AddList(){
		GameObject go=GameObject.Instantiate(listItem,null) as GameObject;
		go.transform.parent=grid.transform;
		go.transform.localScale=Vector3.one;
		go.transform.localPosition=Vector3.one;
		go.transform.localRotation=Quaternion.identity;

		ShipColorScrollItem si;
		si=go.GetComponent<ShipColorScrollItem>();
		si.shipColorLists=this;
		mItemLists.Add(si);
		grid.Reposition();
	}
		

	void TrimItems(int sabun){
		if(sabun<0){
			//足りない
			for(int i=0;i<Mathf.Abs(sabun);i++){
				AddList();
			}

		}else if(sabun>0){
			//余り
			for(int i=0;i<sabun;i++){
				
				mItemLists[i].KillSelf();
				mItemLists.RemoveAt(i);
			}
		}


	}
	#endregion


	#region  Public関数
	public void SetItems(int shipNum){

		//リストと比べて足りないものを足す、余っていたら消す
		string[] subNames=new string[0];
		switch(shipNum+1){
			case 1:
				subNames=PSParams.GameParameters.shipSubNamesShip1;
				break;
			case 2:
				subNames=PSParams.GameParameters.shipSubNamesShip2;
				break;
			case 3:
				subNames=PSParams.GameParameters.shipSubNamesShip3;
				break;
			case 4:
				subNames=PSParams.GameParameters.shipSubNamesShip4;
				break;
			case 5:
				subNames=PSParams.GameParameters.shipSubNamesShip5;
				break;
			case 6:
				subNames=PSParams.GameParameters.shipSubNamesShip6;
				break;
			case 7:
				subNames=PSParams.GameParameters.shipSubNamesShip7;
				break;
			case 8:
				subNames=PSParams.GameParameters.shipSubNamesShip8;
				break;
		}

		if(subNames.Length==0 || shipNum>=PSParams.GameParameters.shipNames.Length){
			Debug.LogError("Not match error");
			return;
		}

		mShipSubNames=subNames;

		int sabun=mItemLists.Count-subNames.Length;
		TrimItems(sabun);

		sabun=mItemLists.Count-subNames.Length;
		if(sabun!=0){
			Debug.LogError("差分がおかしい");
			return;
		}

		ShipColorScrollItem[] items=mItemLists.ToArray();
		for(int e=0;e<mItemLists.Count;e++){
			if(items[e]!=null){
				items[e].SetSprite((shipNum+1).ToString()+"-"+(e+1).ToString());	
				items[e].gameObject.name=e.ToString();
				items[e].SetState(e==0?true:false);
				if(shipNum==DataManager.Instance.gameData.shipType && e==DataManager.Instance.gameData.shipColor){
					items[e].SetOnBoad(true);
				}else{
					items[e].SetOnBoad(false);
				}
			}else{
				Debug.LogError("Null Items "+e);
			}
		}

		mCurrentSelected=0;
		//名前を変更する
		switcher.SetCurrentShipSubName(mShipSubNames[mCurrentSelected]);
		scroll.ResetPosition();

	}

	public void UpdateOnBoad(){
		if(DataManager.Instance.gameData.shipType!=switcher.currentSelect){
			return;
		}
		ShipColorScrollItem[] items=mItemLists.ToArray();
		for(int e=0;e<mItemLists.Count;e++){
			if(items[e]!=null){
				if(int.Parse(items[e].gameObject.name)==DataManager.Instance.gameData.shipColor){
					items[e].SetOnBoad(true);
				}else{
					items[e].SetOnBoad(false);
				}
			}else{
				Debug.LogError("Null Items "+e);
			}
		}
	}

	public void OnClickItem(ShipColorScrollItem item){
		int num=int.Parse(item.gameObject.name);
		if(num!=mCurrentSelected){
			Debug.Log("Color Change");

			mItemLists[mCurrentSelected].SetState(false);

			mCurrentSelected=num;
			mItemLists[mCurrentSelected].SetState(true);

			//TODO Shipのテクスチャを変更する




			//名前を変更する
			switcher.SetCurrentShipSubName(mShipSubNames[mCurrentSelected]);
		}
	}
	#endregion

}
