﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Colorful;

public class FlagSelecter : MonoBehaviour {


	public UILabel mCurrentServer;
	public UISprite mCurrentFlag;

	public delegate void Callback_closedEvent();
	public  event Callback_closedEvent closedEvent;

	void SetServer(string key){
		mCurrentServer.text=key;
		if(Countly.ServerRegionAvaillable[key]){
			mCurrentServer.color=Color.white;
			mCurrentServer.effectColor=mCurrentServer.color;
			current=key;
		}else{
			mCurrentServer.color=Color.grey;
			mCurrentServer.effectColor=mCurrentServer.color;
		}

	}
	public void OnClickNext(){
		currentServerNum++;
		if(currentServerNum>=Countly.ServerRegions.Length){
			currentServerNum=0;
		}
		SetServer(Countly.ServerRegions[currentServerNum]);

	}
	public void OnClickPrev(){
		currentServerNum--;
		if(currentServerNum<0){
			currentServerNum=Countly.ServerRegions.Length-1;
		}
		SetServer(Countly.ServerRegions[currentServerNum]);
	}
	int currentServerNum=0;
	string current="";
	public GameObject btnBG;
	public GameObject container;

	public void Show(){
		AudioController.Play("open");
		btnBG.SetActive(true);
		container.SetActive(true);
		closedEvent=null;
		currentServerNum=System.Array.IndexOf (Countly.ServerRegions, DataManager.Instance.envData.serverRegion);

		current=DataManager.Instance.envData.serverRegion;
		mCurrentServer.text=current;
		UpdateCurrentFlag();
		if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.HideBanner();
	}

	public void ShowWithCallback(Callback_closedEvent closedEvent){
		AudioController.Play("open");
		btnBG.SetActive(true);
		container.SetActive(true);
		this.closedEvent=closedEvent;
		currentServerNum=System.Array.IndexOf (Countly.ServerRegions, DataManager.Instance.envData.serverRegion);

		current=DataManager.Instance.envData.serverRegion;
		mCurrentServer.text=current;
		UpdateCurrentFlag();
		if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.HideBanner();
	}


	public void OnClose(){
		AudioController.Play("popup");
		btnBG.SetActive(false);
		container.SetActive(false);
		if(current!=DataManager.Instance.envData.serverRegion){
			DataManager.Instance.envData.serverRegion=current;
			DataManager.Instance.SaveAll();
			PSPhoton.LobbyManager.instance.OnChangedServer();
		}
		if(DataManager.Instance.gameData.gameTickets!=-100)AdManager.Instance.ShowBanner();

		if(closedEvent!=null)closedEvent();
	}

	public UIGrid grid;

	public GameObject flagObj;

	FlagItem currentSelected;

	public UISprite flagSp;

	void Start(){

		string defaultCountly=DataManager.Instance.gameData.country;
		for(int i=0;i<flags.Length;i++){
			if(flags[i].Equals(defaultCountly)){
				currentSelected=AddNewFlagItem(flags[i],false);
			}else{
				AddNewFlagItem(flags[i],true);
			}
		}

	}

	void UpdateCurrentFlag(){
		mCurrentFlag.spriteName=DataManager.Instance.gameData.country;
	}

	public void OnClickItem(FlagItem item){
		if(item!=currentSelected){
			currentSelected.SetState(true);
			item.SetState(false);
			currentSelected=item;
			DataManager.Instance.gameData.country=item.name;
			PSPhoton.LobbyManager.instance.OnCountlyChanged(item.name);
			flagSp.spriteName=DataManager.Instance.gameData.country=item.name;
			DataManager.Instance.SaveAll();
			UpdateCurrentFlag();
		}
	}
	FlagItem  AddNewFlagItem(string flagName,bool state){
		GameObject go=GameObject.Instantiate(flagObj,grid.transform) as GameObject;
		go.transform.parent=grid.transform;
		go.transform.position=Vector3.zero;
		go.transform.localRotation=Quaternion.identity;
		go.transform.localScale=Vector3.one;

		FlagItem item=go.GetComponent<FlagItem>();
		item.Init(flagName,OnClickItem,state);
		return item;
	}


	#region flagString
	string[] flags=new string[]{"ad" 
		,"ae" 
		,"af" 
		,"ag" 
		,"ai" 
		,"al" 
		,"am" 
		,"an" 
		,"ao" 
		,"ar" 
		,"as" 
		,"at" 
		,"au" 
		,"aw" 
		,"ax" 
		,"az" 
		,"ba" 
		,"bb" 
		,"bd" 
		,"be" 
		,"bf" 
		,"bg" 
		,"bh" 
		,"bi" 
		,"bj" 
		,"bm" 
		,"bn" 
		,"bo" 
		,"br" 
		,"bs" 
		,"bt" 
		,"bv" 
		,"bw" 
		,"by" 
		,"bz" 
		,"ca" 
		,"catalonia" 
		,"cc" 
		,"cd" 
		,"cf" 
		,"cg" 
		,"ch" 
		,"ci" 
		,"ck" 
		,"cl" 
		,"cm" 
		,"cn" 
		,"co" 
		,"cr" 
		,"cs" 
		,"cu" 
		,"cv" 
		,"cx" 
		,"cy" 
		,"cz" 
		,"de" 
		,"dj" 
		,"dk" 
		,"dm" 
		,"do" 
		,"dz" 
		,"ec" 
		,"ee" 
		,"eg" 
		,"eh" 
		,"england" 
		,"er" 
		,"es" 
		,"et" 
		,"eu" 
		,"fi" 
		,"fj" 
		,"fk" 
		,"fm" 
		,"fo" 
		,"fr" 
		,"ga" 
		,"galicia" 
		,"gb" 
		,"gd" 
		,"ge" 
		,"gf" 
		,"gg" 
		,"gh" 
		,"gi" 
		,"gl" 
		,"gm" 
		,"gn" 
		,"gp" 
		,"gq" 
		,"gr" 
		,"gs" 
		,"gt" 
		,"gu" 
		,"gw" 
		,"gy" 
		,"hk" 
		,"hm" 
		,"hn" 
		,"hr" 
		,"ht" 
		,"hu" 
		,"id" 
		,"ie" 
		,"il" 
		,"im" 
		,"in" 
		,"io" 
		,"iq" 
		,"ir" 
		,"is" 
		,"it" 
		,"je" 
		,"jm" 
		,"jo" 
		,"jp" 
		,"ke" 
		,"kg" 
		,"kh" 
		,"ki" 
		,"km" 
		,"kn" 
		,"kp" 
		,"kr" 
		,"kw" 
		,"ky" 
		,"kz" 
		,"la" 
		,"lb" 
		,"lc" 
		,"li" 
		,"lk" 
		,"lr" 
		,"ls" 
		,"lt" 
		,"lu" 
		,"lv" 
		,"ly" 
		,"ma" 
		,"mc" 
		,"md" 
		,"me" 
		,"mf" 
		,"mg" 
		,"mh" 
		,"mk" 
		,"ml" 
		,"mm" 
		,"mn" 
		,"mo" 
		,"mp" 
		,"mq" 
		,"mr" 
		,"ms" 
		,"mt" 
		,"mu" 
		,"mv" 
		,"mw" 
		,"mx" 
		,"my" 
		,"mz" 
		,"na" 
		,"nc" 
		,"nc2" 
		,"ne" 
		,"nf" 
		,"ng" 
		,"ni" 
		,"nl" 
		,"no" 
		,"np" 
		,"nr" 
		,"nu" 
		,"nz" 
		,"om" 
		,"pa" 
		,"pe" 
		,"pf" 
		,"pg" 
		,"ph" 
		,"pk" 
		,"pl" 
		,"pm" 
		,"pn" 
		,"pr" 
		,"ps" 
		,"pt" 
		,"pw" 
		,"py" 
		,"qa" 
		,"re" 
		,"ro" 
		,"rs" 
		,"ru" 
		,"rw" 
		,"sa" 
		,"sb" 
		,"sc" 
		,"scotland" 
		,"sd" 
		,"se" 
		,"sg" 
		,"sh" 
		,"si" 
		,"sj" 
		,"sk" 
		,"sl" 
		,"sm" 
		,"sn" 
		,"so" 
		,"sr" 
		,"st" 
		,"sv" 
		,"sy" 
		,"sz" 
		,"tc" 
		,"td" 
		,"tf" 
		,"tg" 
		,"th" 
		,"tj" 
		,"tk" 
		,"tl" 
		,"tm" 
		,"tn" 
		,"to" 
		,"tr" 
		,"tt" 
		,"tv" 
		,"tw" 
		,"tz" 
		,"ua" 
		,"ug" 
		,"um" 
		,"us" 
		,"uy" 
		,"uz" 
		,"va" 
		,"vc" 
		,"ve" 
		,"vg" 
		,"vi" 
		,"vn" 
		,"vu" 
		,"wales" 
		,"wf" 
		,"ws" 
		,"ye" 
		,"yt" 
		,"za" 
		,"zm" 
		,"zw"
	};
	#endregion

}
