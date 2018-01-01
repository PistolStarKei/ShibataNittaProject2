using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Subweapon{NAPAM,NUKE,RAZER,STEALTH,WAVE,YUDOU,ZENHOUKOU,NONE}

public class SubWeaponMenu : MonoBehaviour {
	

	public Subweapon subTest;
	public void Test(){
		AddSubWeaponToHolder(subTest);
	}

	public void Test2(){
		OnUseSubWeapon();
	}



	public void OnTapItem(){
		if(GetCurrentWeapon()!=null && GetCurrentWeapon()!=Subweapon.NONE){
			GUIManager.Instance.OnUseSubWeapon(GetCurrentWeapon());
		}
		OnUseSubWeapon();
	}

	public List<Subweapon> subWeaponHolder=new List<Subweapon>();

	public bool ISHolderHasSpace(){
		if(subWeaponHolder.Count>=10){
			return false;
		}else{
			return true;
		}
	}

	public Subweapon GetCurrentWeapon(){
		if(subWeaponHolder.Count<=0){
			return Subweapon.NONE;
		}
			
		return subWeaponHolder[subWeaponHolder.Count-1];

	}

	public void AddSubWeaponToHolder(Subweapon add){
		if(!ISHolderHasSpace()){
			return;
		}

		subWeaponHolder.Add(add);
		UpdateBtns();
	}

	void OnUseSubWeapon(){
		if(subWeaponHolder.Count<=0){
			return;
		}

		subWeaponHolder.RemoveAt(subWeaponHolder.Count-1);

		UpdateBtns();
	}


	void UpdateBtns(){

		holdNum.text=subWeaponHolder.Count.ToString()+"/10";

		SetCurrent(subWeaponHolder.Count<=0 ? Subweapon.NONE:subWeaponHolder[subWeaponHolder.Count-1]);

		for(int i=0;i<nextHolder.Length;i++){
			nextHolder[i].SetToEmpty();
		}

		int y=0;
		for(int i=subWeaponHolder.Count-2;i>=0;i--){
			nextHolder[y].SetItem(fittingName[(int)subWeaponHolder[i]]);
			y++;
		}
	}

	public UISprite currentSub;


	void SetCurrent(Subweapon wep){

		if(wep==Subweapon.NONE){
			currentSub.enabled=false;
			ready.enabled=false;
			SetHighLight(false);
		}else{
			
			SetCurrentSubItem(wep);
		}
	}


	public SubweaponNext[] nextHolder;

	string[] fittingName=new string[7]{"UI_Napam","UI_Nuke","UI_Razer","UI_Stealth","UI_Wave","UI_Yudou","UI_Zenhoukou"};

	public TweenPosition left;
	public TweenPosition right;

	void SetHighLight(bool isOn){
		if(isOn){
			if(left.enabled==false){
				left.transform.localPosition=left.from;
				right.transform.localPosition=right.from;
				left.enabled=true;
				right.enabled=true;
			}
		}else{
			if(left.enabled==true){
				left.enabled=false;
				right.enabled=false;
				left.transform.localPosition=left.from;
				right.transform.localPosition=right.from;

			}
		}
	}

	public UILabel holdNum;
	public UILabel ready;

	void SetCurrentSubItem(Subweapon wep){
		
		currentSub.enabled=true;
		ready.enabled=true;
		currentSub.spriteName=fittingName[(int)wep];
		SetHighLight(true);
	}
}
