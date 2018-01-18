using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : PS_SingletonBehaviour<GUIManager> {

	public bool isDebugMode=true;
	// Use this for initialization
	public void OnGameAwake(){
		SetKills(0);
		DisableAllGUI(false);
	}

	public void OnGameStart(){
		AbleAllGUI();
	}

	public ResultPanel resultMenu;
	public void OnGameOver(float time,int killNum,List<shipControl> deadShips,List<shipControl> ships,shipControl playerShip){
		resultMenu.ShowResult(time,killNum,deadShips,ships,playerShip);
	}

	MapDetecterTrigger shipSearcher;

	public shipControl GetNearestShip(Vector3 pos,float maxDistance){
		return shipSearcher.GetNearestShip(pos,maxDistance);
	}
	/// <summary>
	/// 近いshipをランダムで返します。shipは、マップに入っているものの中から、死んでいないものを返します。posには、現在の発射したshipのポジションを、maxDistanceには、最大範囲を指定します。
	/// </summary>
	public shipControl GetRandomNearShip(Vector3 pos,float maxDistance){
		return shipSearcher.GetNearestShip(pos,maxDistance);
	}

	[HideInInspector]
	public shipControl shipControll;
	public void SetShipControll(shipControl shipControll){
		//カメラに機体を設定
		Camera.main.gameObject.GetComponent<cameraLookAt>().target=shipControll.gameObject.transform;
		this.shipControll=shipControll;

		GameObject go=GameObject.FindGameObjectWithTag("ShipSercher");
		if(go){
			shipSearcher=go.GetComponent<MapDetecterTrigger>();
			if(shipSearcher){
				shipSearcher.playerTrans=shipControll.gameObject.transform;
			}else{
				Debug.LogWarning("ShipSercher がありません");
			}

		}else{
			Debug.LogWarning("ShipSercher がありません");
		}
	}

	/*
	 * 
	 * タップ受け取り
	 * 
	 * 
	*/
	public void OnPressTapLayer(bool isPress,Vector3 worldPos){
		if(shipControll!=null)shipControll.OnPressTapLayer(isPress,worldPos);

	}

	public void OnUpdateTapLayer(Vector3 worldPos){
		if(shipControll!=null)shipControll.OnUpdateTapLayer(worldPos);
	}

	/*
	 * 
	 * Shootボタン
	 * 
	 * 
	*/
	public void OnShootBtnClicked(){
		
	}

	public ShotToggleBtn shotTgl;
	public void SetShotTgl(bool val){
		shotTgl.SetToggle(val);
	}
	public bool OnShootBtnToggle(bool val){
		return shipControll.OnShotToggle(val);
	}
		
	public Subweapon subs=Subweapon.NAPAM;
	public void Test(){
		OnUseSubWeapon(subs);
	}





	/*
	 * 
	 * HP スライダー
	 * 
	 * 
	*/
	public PS_GUI_HPSlider hpSlider;
	//ダメージ時に、最大HPと、ダメージ量を与える
	public void Damage(float damageVal,float MaxHP){
		if(damageVal<0.0f){
			Debug.LogError("damageVal not to be minus");
			return;
		}
		if(damageVal>MaxHP){
			damageVal=MaxHP;
		}
		hpSlider.MinusVal(damageVal/MaxHP);



	}

	//回復時に、最大HPと、回復割合を1-100%で与える
	public void Cure(float persentage,float MaxHP){
		if(persentage<0.0f){
			Debug.LogError("persentage not within 1-100%");
			persentage=1.0f;
		}
		hpSlider.AddVal((MaxHP*(persentage/100.0f))/MaxHP);

	}

	/*
	 * 
	 * サブウェポン
	 * 
	 * 
	*/

	public SubWeaponMenu subWeaponSlot;

	public int[] GetSubweaponInHolder(){
		return  subWeaponSlot.GetAllWeaponInHolder();
	}
	public bool ISSubweponHolderHasSpace(){
		return  subWeaponSlot.ISHolderHasSpace();

	}

	/// <summary>
	/// サブウェポン取得時にGUIを更新する　もしも最大個数であればfalseを返す
	/// </summary>
	public bool OnGetSubWeapon(Subweapon weaponType){
		if(!subWeaponSlot.ISHolderHasSpace()){
			Debug.Log(" ホルダーがいっぱいでこれ以上は持てない ");
			return false;
		}

		subWeaponSlot.AddSubWeaponToHolder(weaponType);

		return true;
	}

	public void OnUseSubWeapon(Subweapon weaponType){
		shipControll.OnUseSubWeapon(weaponType);
	}
	public void EnableSubweapon(){
		subWeaponSlot.OnUsedSubweapon();
	}

	/*
	 * 
	 * 残機とキル
	 * 
	 * 
	*/
	public PS_GUI_LabelAnimation zankiLAbel;
	public void SetZanki(string str){
		zankiLAbel.SetNum(str);
	}
	//TODO ローカライズする。
	public PS_GUI_LabelAnimation killLAbel;
	public void SetKills(int killNum){
		Debug.Log("SetKills"+killNum);
		killLAbel.SetNum(killNum.ToString());
	}

	/*
	 * 
	 * ログ
	 * 
	 * 
	*/
	public PS_GUI_DynamicInfo logger;
	public void Log(string log){
		logger.Log(log);
	}

	/*
	 * 
	 * カウントダウン
	 * 
	 * 
	*/
	public UILabel countdownLb;
	public void SetCountdown(string str){
		countdownLb.text=str;
	}

	/*
	 * 
	 * GUIカバー
	 * 
	 * 
	*/
	public PS_GUI_Cover guiCover;

	/*
	 * 
	 * GUIカバー
	 * 
	 * 
	*/
	public void DisableAllGUI(bool isBlackMask){
		if(isBlackMask){
			guiCover.CoverWithBlackMask();
		}else{
			guiCover.Cover();
		}
	}
	public void AbleAllGUI(){
		guiCover.Uncover();
	}

	public SettingMenu settingMenu;
	public void SetSettingValues(bool se,bool bgm){
		settingMenu.SetSettignValues(se,bgm);
	}





}
