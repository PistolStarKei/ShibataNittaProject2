using UnityEngine;
using System.Collections;

public class PS_GUI_ScrollIndicator : MonoBehaviour {

	public GameObject top;
	public GameObject btm;

	public UIScrollView view;

	void Start(){
		SetActive(btm,false);
		SetActive(top,false);
	}
	void SetActive(GameObject go,bool flag){
		if(go.activeSelf!=flag)NGUITools.SetActive(go,flag);
	}

	public float topMin=0.1f;
	public float btmMax=0.9f;

	void FixedUpdate(){

		//0 top 1 btm
		if(view.gameObject.activeSelf){
			if( view.verticalScrollBar.value<=topMin){
				//top 下がまだある
				SetActive(top,false);
				SetActive(btm,true);
			}else{
				SetActive(top,true);
				if(view.verticalScrollBar.value>=btmMax){
					//btm
					SetActive(btm,false);
				}else{
					SetActive(btm,true);
				}
			}
		}else{
			SetActive(btm,false);
			SetActive(top,false);
		}

	}


}
