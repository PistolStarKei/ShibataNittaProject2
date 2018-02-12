using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// WaitHUDの説明
/// </summary>

namespace PSGUI{
	public class WaitHUD : MonoBehaviour {

		public static WaitHUD guiWait;
		#region  メンバ変数
		public UIPanel mPanel;
		public BoxCollider mCollider;
		public Transform mTweenRot1;
		public Transform mTweenRot2;
		public UILabel mLb;
		#endregion

		public float rotaterDulation=1.0f;
		public bool startCover=false;

		#region  初期化

		void Awake () {
			guiWait=this;
			mPanel=gameObject.GetComponent<UIPanel>();
			if(!mPanel)Debug.LogError("GetComponent null");
		}

		void Start(){
			if(startCover){
				Show(13,"Loading");
			}
		}
		#endregion

		#region  Public関数
		public void Show(int width){
			//カバーで覆う
			this.mPanel.depth=width+1;

			NGUITools.SetActive(mCollider.gameObject,true);
			NGUITools.SetActive(mTweenRot1.gameObject,true);
			NGUITools.SetActive(mTweenRot2.gameObject,true);
			NGUITools.SetActive(mLb.gameObject,false);
			mLb.text="";

			rotC=StartCoroutine(Rotate());

		}
		Coroutine rotC;

		bool isInivokin=false;
		IEnumerator Rotate(){
			if(isInivokin)yield break;
			isInivokin=true;
			while(true){
				mTweenRot1.transform.Rotate(Vector3.forward * Time.deltaTime*rotaterDulation);
				mTweenRot2.transform.Rotate(-Vector3.forward * Time.deltaTime*rotaterDulation*5f);
				yield return null;
			}

		}


		public void Show(int width,string labelString){
			//カバーで覆う
			this.mPanel.depth=width+1;

			NGUITools.SetActive(mCollider.gameObject,true);
			mLb.enabled=true;
			NGUITools.SetActive(mTweenRot1.gameObject,true);

			NGUITools.SetActive(mTweenRot2.gameObject,true);

			NGUITools.SetActive(mLb.gameObject,true);
			mLb.text=labelString;
			rotC=StartCoroutine(Rotate());
			
		}
		public void Hide(){
			if(rotC!=null && isInivokin){
				isInivokin=false;
				NGUITools.SetActive(mCollider.gameObject,false);
				NGUITools.SetActive(mLb.gameObject,false);
				NGUITools.SetActive(mTweenRot1.gameObject,false);
				NGUITools.SetActive(mTweenRot2.gameObject,false);
				StopCoroutine(rotC);
				rotC=null;
			}
		}

		#endregion

	}

}