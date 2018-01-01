using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Subweapon{NAPAM,NUKE,RAZER,STEALTH,WAVE,YUDOU,ZENHOUKOU,NONE}

public class SubWeaponMenu : MonoBehaviour {
	

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
		return subWeaponHolder[0];

	}

	public void AddSubWeaponToHolder(Subweapon add){
		if(!ISHolderHasSpace()){
			Debug.LogError("Holder is Full");
			return;
		}

		subWeaponHolder.Add(add);
		UpdateBtns();
	}

	void OnUseSubWeapon(){
		if(subWeaponHolder.Count<=0){
			Debug.LogError("Holder is Empty");
			return;
		}
		subWeaponHolder.RemoveAt(0);
		UpdateBtns();
	}


	void UpdateBtns(){
		holdNum.text=subWeaponHolder.Count.ToString()+"/10";
		SetCurrent(subWeaponHolder[0]!=null ? subWeaponHolder[0] : Subweapon.NONE);

		for(int i=1;i<nextHolder.Length+1;i++){
			if(subWeaponHolder[i]==null || subWeaponHolder[i]==Subweapon.NONE){

				nextHolder[i-1].SetToEmpty();
			}else{
				nextHolder[i-1].SetItem(fittingName[(int)subWeaponHolder[i]]);
			}
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
