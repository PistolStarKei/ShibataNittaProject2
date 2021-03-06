﻿using UnityEngine;
using System.Collections;

public class SphericalIndicaterItem : MonoBehaviour {

	public UISprite bgSp;
	UISprite mSp;
	void SetBGOn(bool isOn){
		if(parentIndicator==null)FindParent ();
		if(mSp==null)mSp=gameObject.GetComponent<UISprite>();
		if(bgSp==null)Debug.LogError("bgSP==ull");
		if(parentIndicator==null)Debug.LogError("parentIndicator==ull");

		bgSp.enabled=isOn;
		bgSp.color=parentIndicator.activeCol;
		mSp.color=isOn? parentIndicator.activeCol: parentIndicator.disactiveCol;
	}
	public SphericalIndicator parentIndicator;
	public void FindParent ()
	{
		
		SphericalIndicator pi = transform.parent.gameObject.GetComponent<SphericalIndicator>();
		if(pi==null)Debug.LogError("pi==null");
		parentIndicator = pi;
	}
	public bool mIson=false;
	// Use this for initialization
	void Awake () {
		mSp=gameObject.GetComponent<UISprite>();
		FindParent ();
		SetBGOn(mIson);
	}

	public void SetOn(bool isOn){
		if(mIson!=isOn){
			mIson=isOn;
			SetBGOn(isOn);
		}

	}
}
