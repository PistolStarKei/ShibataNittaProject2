using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SphericalIndicator : MonoBehaviour {

	public UIGrid mGrid;
	public List<SphericalIndicaterItem> mItems=new List<SphericalIndicaterItem>();
	public Color activeCol;
	public Color disactiveCol;

	public GameObject item;
	void AddItem(){
		GameObject go=GameObject.Instantiate(item,mGrid.transform) as GameObject;
		go.transform.localPosition=Vector3.one;
		go.transform.localRotation=Quaternion.identity;
		go.transform.localScale=Vector3.one;
	}
	void Trim(){
		int num=PSParams.GameParameters.shipNames.Length;
		int child=transform.childCount;

		if(child!=num){
			if(child<num){
				//足りない
				for(int i=0;i<num-child;i++){
					AddItem();
				}
			}else{
				//余る
				for(int i=0;i<child-num;i++){
					Destroy(transform.GetChild(i).gameObject);
				}
			}
			Debug.Log("生成");
			mGrid.Reposition();
		}


	}

	// Use this for initialization
	void Awake () {
		mGrid=gameObject.GetComponent<UIGrid>();
		//動的に生成する
		Trim();

		GetItems();


		mCurrent=0;
	}
		
	void GetItems(){
		SphericalIndicaterItem it;
		foreach(Transform trans in transform){
			it=trans.gameObject.GetComponent<SphericalIndicaterItem>();
			if(it!=null){
				mItems.Add(it);
			}
		}
	}

	public void SetCurrent(int setC){
		mCurrent=setC;
		int i=0;
		foreach(SphericalIndicaterItem si in mItems){
			
			if(i==mCurrent){
				si.SetOn(true);
			}else{
				si.SetOn(false);
			}
			i++;
		}
	}
		
	public int mCurrent=0;

	public delegate void Callback_OnChanged(int num);
	public event Callback_OnChanged onChanged;
	public void OnPrev(){
		
		if(mCurrent-1<0){
			mCurrent=mItems.Count-1;
			//if(mNextBtn.gameObject.activeSelf)NGUITools.SetActive(mNextBtn,false);

		}else{
			mCurrent--;
			//if(mPrevBtn.gameObject.activeSelf)NGUITools.SetActive(mPrevBtn,true);
		}
		if(onChanged!=null)onChanged(mCurrent);
		SetCurrent(mCurrent);
	}
	public void OnNext(){
		
		if(mCurrent+1>=mItems.Count){
			mCurrent=0;
			//if(mNextBtn.gameObject.activeSelf)NGUITools.SetActive(mNextBtn,false);

		}else{
			mCurrent++;
			//if(mPrevBtn.gameObject.activeSelf)NGUITools.SetActive(mPrevBtn,true);
		}
		if(onChanged!=null)onChanged(mCurrent);
		SetCurrent(mCurrent);
	}
}
