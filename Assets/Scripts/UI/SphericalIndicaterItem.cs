using UnityEngine;
using System.Collections;

public class SphericalIndicaterItem : MonoBehaviour {

	public UISprite bgSp;
	UISprite mSp;
	void SetBGOn(bool isOn){
		bgSp.enabled=isOn;
		bgSp.color=parentIndicator.activeCol;
		mSp.color=isOn? parentIndicator.activeCol: parentIndicator.disactiveCol;
	}
	public SphericalIndicator parentIndicator;
	void FindParent ()
	{
		// If the scroll view is on a parent, don't try to remember it (as we want it to be dynamic in case of re-parenting)
		SphericalIndicator pi = NGUITools.FindInParents<SphericalIndicator>(transform);
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
