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
		colorLists.SetItems(currentSelect);

	}

	public BoadingBtn boadingBtn;

	public int currentSelectColor=0;
	public void OnShipColorChanged(){

		//Lobbyに通知する
		lobby.OnShipChanged(currentSelect,currentSelectColor);

		//データ保存
		DataManager.Instance.gameData.shipType=currentSelect;
		DataManager.Instance.gameData.shipColor=currentSelectColor;
		DataManager.Instance.SaveAll();
		//カラーリストも更新
		colorLists.UpdateOnBoad();
	}

	public int currentSelect=0;
	public void OnShipChanged(int num){
		currentSelect=num;
		SetCurrentShipName(shipNames[num]);
		switcher.Set(num);
		colorLists.SetItems(currentSelect);
	}


	public UILabel currentShipNameLb;
	public void SetCurrentShipName(string name){
		currentShipNameLb.text=name;

	}

	public UILabel currentShipSubNameLb;
	public void SetCurrentShipSubName(string name){
		currentShipSubNameLb.text=name;

		UpdateBoadingBtn();
	}

	public void UpdateBoadingBtn(){
		if(DataManager.Instance.gameData.shipType==currentSelect && DataManager.Instance.gameData.shipColor==colorLists.mCurrentSelected){
			boadingBtn.SetState(ShipStatus.BOADING,"");
			return;
		}

		bool[] flags=DataManager.Instance.GetAvaillableFlagFromData(currentSelect);

		if(flags==null || flags.Length==0){
			Debug.LogError("Nullエラー");
			return;
		}

		//搭乗可能機の判断　購入済なら良い
		if(flags[colorLists.mCurrentSelected]){
			//セレクト可能
			boadingBtn.SetState(ShipStatus.SELECTABLE,"");
		}else{
			Debug.LogWarning("ここで購入処理 金額を渡す");
			//TODO ここで購入処理 金額を渡す
			boadingBtn.SetState(ShipStatus.PURCHASABLE,"");
		}

	}

	public void OnClickBoadningBtn(){
		if(DataManager.Instance.gameData.shipType==currentSelect && DataManager.Instance.gameData.shipColor==colorLists.mCurrentSelected){
			//現在の搭乗機なので無効にする
			return;
		}

		bool[] flags=DataManager.Instance.GetAvaillableFlagFromData(currentSelect);

		if(flags==null || flags.Length==0){
			Debug.LogError("Nullエラー");
			return;
		}

		//搭乗可能機の判断　購入済なら良い
		if(flags[colorLists.mCurrentSelected]){
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == SystemLanguage.Japanese? "搭乗機の変更が完了しました" :"Bording new ship complete");
			AudioController.Play("Enter");
			//セレクト可能
			currentSelectColor=colorLists.mCurrentSelected;
			OnShipColorChanged();
			UpdateBoadingBtn();
		}else{
			//TODO ここで購入処理
			Debug.LogWarning("ここで購入処理 金額を渡す");
		}




	}



}
