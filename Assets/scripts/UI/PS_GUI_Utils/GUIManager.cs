using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : PS_SingletonBehaviour<GUIManager> {

	public LightningEffect lightningFX;
	public void ShowLightnings(bool isShow){
		if(isShow){
			lightningFX.ShowLightnings();
		}else{
			lightningFX.HideLightnings();
		}
	}
	public Thinksquirrel.CShake.CameraShake cameraShaker;

	public void ShakeCamera(){
		cameraShaker.numberOfShakes=3;
		cameraShaker.shakeAmount=new Vector3(0.4f,0.0f,0.4f);
		cameraShaker.rotationAmount=new Vector3(0.4f,0.4f,0.4f);
		cameraShaker.Shake();
	}
	public void ShakeCameraBig(){
		cameraShaker.numberOfShakes=5;
		cameraShaker.shakeAmount=new Vector3(0.8f,0.0f,0.8f);
		cameraShaker.rotationAmount=new Vector3(0.8f,0.8f,0.8f);
		cameraShaker.Shake();
	}

	public bool isDebugMode=true;
	// Use this for initialization
	public void OnGameAwake(){
		Debug.Log("OnGameAwake");
		SetKills(0);
		DisableAllGUI(false);
	}

	public void OnGameStart(){
		Debug.Log("OnGameStart");
		AbleAllGUI();
	}

	public ResultPanel resultMenu;
	public void OnGameOver(float time,int killNum,int playerRank,int playerCount,shipControl playerShip){
		resultMenu.ShowResult(time,killNum,playerRank,playerCount,playerShip);
	}


	MapDetecterTrigger shipSearcher;

	public shipControl GetNearestShip(Vector3 pos,float maxDistance){
		return shipSearcher.GetNearestShip(pos,maxDistance);
	}
	/// <summary>
	/// 近いshipをランダムで返します。shipは、マップに入っているものの中から、死んでいないものを返します。maxDistanceには、最大範囲を指定します。
	/// </summary>
	public shipControl GetRandomNearShip(float maxDistance){
		return shipSearcher.GetRandomNearShip(maxDistance);
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
		
	public Pickup subs=Pickup.NAPAM;


	public void Test(){
		if(subs==Pickup.CureL || subs==Pickup.CureM || subs==Pickup.CureS){
			shipControll.OnPickUp_Cure(subs);
		}else{
			shipControll.OnPickUp_Subweapon(subs);
		}
	}

	public Subweapon usesubweapon=Subweapon.NAPAM;
	public void Test2(){
		shipControll.OnUseSubWeapon(usesubweapon);
	}
	public void Test3(){
		shipControll.ScatterWeapons(subWeaponSlot.GetAllWeaponInHolder(),shipControll.shot_rensou);
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
		Debug.Log("OnUseSubWeapon "+weaponType.ToString());
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
	public StartCountDown countdown;
	public void SetCountdown(int str){
		if(countdown!=null)countdown.SetCount(str);
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
