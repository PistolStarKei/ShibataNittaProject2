﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class SubWeaponMenu : MonoBehaviour {
	

	public Subweapon subTest;
	public TweenRotation tr;
	public UILabel subWeaponNameLb;

	UIWidget widget;
	void Start(){
		widget=gameObject.GetComponent<UIWidget>();
	}

	public void OnUsedSubweapon(){
		widget.alpha=1.0f;
	}

	public void OnTapItem(){
		if(GetCurrentWeapon()!=null && GetCurrentWeapon()!=Subweapon.NONE){
			if(GUIManager.Instance.shipControll!=null){
				if(GUIManager.Instance.shipControll.currentUsing==Subweapon.NONE){
					GUIManager.Instance.OnUseSubWeapon(GetCurrentWeapon());
					OnUseSubWeapon();
				}
			}
		}

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

	public int[] GetAllWeaponInHolder(){

		if(subWeaponHolder.Count<=0){
			return null;
		}

		List<int> ret =new List<int>();

		foreach(Subweapon wep in subWeaponHolder){
			ret.Add((int)wep);
		}

		return ret.ToArray();
	}

	public void AddSubWeaponToHolder(Subweapon add){
		
		if(!ISHolderHasSpace()){
			return;
		}
		Debug.Log("AddSubWeaponToHolder " +add);
		subWeaponHolder.Add(add);
		UpdateBtns();
	}

	void OnUseSubWeapon(){
		if(subWeaponHolder.Count<=0){
			return;
		}
		widget.alpha=0.2f;
		subWeaponHolder.RemoveAt(subWeaponHolder.Count-1);

		UpdateBtns();
	}


	void UpdateBtns(){


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
			tr.enabled=false;
			currentSub.enabled=false;
			SetHighLight(false);
			subWeaponNameLb.text="";
		}else{
			subWeaponNameLb.text=Localization.Get(fittingName[(int)wep]);
			SetCurrentSubItem(wep);
		}
	}

	public SubweaponNext[] nextHolder;

	string[] fittingName=new string[7]{"PU_Napam","PU_Nuke","PU_Razer","PU_Stealth","PU_Wave","PU_Yudou","PU_Zenhoukou"};

	void SetHighLight(bool isOn){
		if(isOn){
			
		}else{
			

		}
	}

	void SetCurrentSubItem(Subweapon wep){
		currentSub.enabled=true;
		tr.enabled=true;
		currentSub.spriteName=fittingName[(int)wep];
		SetHighLight(true);
	}
}
