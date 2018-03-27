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
		indecator.SetIndecatorCount(PSParams.GameParameters.shipNames.Length);
		indecator.onChanged+=OnShipChanged;
		currentSelect=DataManager.Instance.gameData.shipType;
		indecator.SetCurrent(currentSelect);
		switcher.SetWithNoAnime(currentSelect);
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

	public UnlockLabel unlockLabel;
	public void OnTimeOut(){
		UpdateBoadingBtn();
	}

	public void UpdateBoadingBtn(){
		if(DataManager.Instance.gameData.shipType==currentSelect && DataManager.Instance.gameData.shipColor==colorLists.mCurrentSelected){
			boadingBtn.SetState(ShipStatus.BOADING,"");
			unlockLabel.SetLastTime();
			return;
		}

		bool[] flags=DataManager.Instance.GetAvaillableFlagFromData(currentSelect);

		if(flags==null || flags.Length==0){
			Debug.LogError("Nullエラー");
			return;
		}



		float keikaSec=TimeManager.Instance.GetKeikaTimeFromStart();

		float[] times=PSParams.GameParameters.unlockTimeShip["Ship"+(currentSelect+1).ToString()];
		if(times.Length<=0)Debug.LogError("Key not found");


		//開放済みかの判断
		if(keikaSec<	times[colorLists.mCurrentSelected]*3600f){
			unlockLabel.SetLastTime((times[colorLists.mCurrentSelected]*3600f)-keikaSec,OnTimeOut);
			boadingBtn.SetState(ShipStatus.PENDINNG,"");
			return;
		}


		//搭乗可能機の判断　購入済なら良い
		if(flags[colorLists.mCurrentSelected]){
			//セレクト可能
			boadingBtn.SetState(ShipStatus.SELECTABLE,"");
			unlockLabel.SetLastTime();
		}else{
			//ここで購入処理 金額を渡す
			unlockLabel.SetLastTime();
			boadingBtn.SetState(ShipStatus.PURCHASABLE,GetShipPrice(currentSelect,colorLists.mCurrentSelected));
		}

	}

	string GetShipPrice(int ship, int color){
		string price="--";

		if(!PS_Plugin.Instance.storeListener.IsStoreAvaillable()){
			price="N/A";
			return price;
		}

		if(ship==0 && color ==3){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship1sc");
		}else if(ship==1 && color ==3){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship2sc");
		}else if(ship==3 && color ==3){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship4sc");
		}else if(ship==5 && color ==2){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship6pan");
		}else if(ship==5 && color ==4){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship6sak");
		}else if(ship==6 && color ==2){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship7met");
		}else if(ship==6 && color ==3){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship7gld");
		}else if(ship==7 && color ==3){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship8gld");
		}else if(ship==7 && color ==4){
			price=PS_Plugin.Instance.storeListener.GetPrice("ship8sc");
		}

		return price;

	}


	public void OnClickBoadningBtn(ShipStatus status){
		if(status==ShipStatus.BOADING || status==ShipStatus.PENDINNG ){
			//現在の搭乗機なので無効にする
			return;
		}

		//搭乗可能機の判断　購入済なら良い
		if(status==ShipStatus.SELECTABLE){
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == SystemLanguage.Japanese? "搭乗機の変更が完了しました" :"Bording new ship complete");
			AudioController.Play("Enter");
			//セレクト可能
			currentSelectColor=colorLists.mCurrentSelected;
			OnShipColorChanged();
			UpdateBoadingBtn();
		}else if(status==ShipStatus.PURCHASABLE){
			//TODO ここで購入処理
			if(PS_Plugin.Instance.storeListener.IsStoreAvaillable()){
				PS_Plugin.Instance.storeListener.PurchaseProduct(GetShipSku(currentSelect,colorLists.mCurrentSelected),OnPurchaseSuccessed,OnPurchaseFailled);
				PSGUI.WaitHUD.guiWait.Show(9,"Connecting");
			}


		}
	}

	string GetShipSku(int ship, int color){
		string price="--";

		if(ship==0 && color ==3){
			price="ship1sc";
		}else if(ship==1 && color ==3){
			price="ship2sc";
		}else if(ship==3 && color ==3){
			price="ship4sc";
		}else if(ship==5 && color ==2){
			price="ship6pan";
		}else if(ship==5 && color ==4){
			price="ship6sak";
		}else if(ship==6 && color ==2){
			price="ship7met";
		}else if(ship==6 && color ==3){
			price="ship7gld";
		}else if(ship==7 && color ==3){
			price="ship8gld";
		}else if(ship==7 && color ==4){
			price="ship8sc";
		}

		return price;

	}


	public void OnPurchaseSuccessed(string id){

		AudioController.Play("successed");
		UpdateBoadingBtn();
		PSGUI.WaitHUD.guiWait.Hide();
		PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
			SystemLanguage.Japanese? "購入が完了しました。" :"Purchase successed");
	}

	public void OnPurchaseFailled(string mess){

		AudioController.Play("failled");

		PSGUI.WaitHUD.guiWait.Hide();
		if(mess=="購入完了も、消費に失敗　再起動せよ"){
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
				SystemLanguage.Japanese? "購入の反映に失敗しました、ゲームを再起動してください" :"Data affect failled.Reboot application.");
		}else{
			PSPhoton.LobbyManager.instance.info.Log(Application.systemLanguage == 
				SystemLanguage.Japanese? "購入に失敗しました。" :"Purchase failled");
		}


	}

	public bool IsSPContent(int ship, int color){
		bool isSPContent=false;

		if(ship==0 && color ==3){
			isSPContent=true;
		}else if(ship==1 && color ==3){
			isSPContent=true;
		}else if(ship==3 && color ==3){
			isSPContent=true;
		}else if(ship==5 && color ==2){
			isSPContent=true;
		}else if(ship==5 && color ==4){
			isSPContent=true;
		}else if(ship==6 && color ==2){
			isSPContent=true;
		}else if(ship==6 && color ==3){
			isSPContent=true;
		}else if(ship==7 && color ==3){
			isSPContent=true;
		}else if(ship==7 && color ==4){
			isSPContent=true;
		}

		return isSPContent;

	}

}
