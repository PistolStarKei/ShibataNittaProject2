using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : PS_SingletonBehaviour<GUIManager> {
	// Use this for initialization
	void Start () {

	}



	public shipControl shipControll;

	/*
	 * 
	 * タップ受け取り
	 * 
	 * 
	*/
	public void OnPressTapLayer(bool isPress){
		//shipControl.OnClickTapLayer(worldPos);

	}

	public void OnClickTapLayer(Vector3 worldPos){
		//shipControl.OnClickTapLayer(worldPos);

	}

	public void OnDragTapLayer(Vector3 worldPos){
		
	}

	/*
	 * 
	 * Shootボタン
	 * 
	 * 
	*/
	public void OnShootBtnClicked(){
		
	}





	
	public void Test(){
		ShowResult(1,12);
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

	/// <summary>
	/// サブウェポン取得時にGUIを更新する　もしも最大個数であればfalseを返す
	/// </summary>
	public bool OnGetSubWeapon(Subweapon weaponType){
		if(!subWeaponSlot.ISHolderHasSpace()){
			return false;
		}

		subWeaponSlot.AddSubWeaponToHolder(weaponType);

		return true;
	}

	public void OnUseSubWeapon(Subweapon weaponType){
		
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
		zankiLAbel.SetNum(killNum.ToString()+"Kills");
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

	public SettingMenu settingMenu;
	public void SetSettingValues(bool se,bool bgm){
		settingMenu.SetSettignValues(se,bgm);
	}

	/*
	 * 
	 * Result
	 * 
	 * 
	*/

	public ResultPanel resultMenu;
	public void ShowResult(int rank,int kills){
		resultMenu.ShowResult(rank,kills);
	}

}
