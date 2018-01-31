using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// WaitHUDの説明
/// </summary>

namespace PSGUI{
	public class WaitHUD : MonoBehaviour {

		#region  メンバ変数
		public UIPanel mPanel;
		public UIWidget mCollider;
		public TweenRotation mTweenRot1;
		public TweenRotation mTweenRot2;
		public UILabel mLb;
		#endregion

		public float rotaterDulation=1.0f;
		public bool startCover=false;

		#region  初期化

		void Awake () {
			mPanel=gameObject.GetComponent<UIPanel>();
			if(!mPanel)Debug.LogError("GetComponent null");
		}
		#endregion

		#region  Public関数
		public void Show(int width){
			//カバーで覆う
			this.mPanel.depth=width+1;

			mCollider.enabled=true;
			NGUITools.SetActive(mTweenRot1.gameObject,true);

			mTweenRot1.duration=rotaterDulation;
			NGUITools.SetActive(mTweenRot2.gameObject,true);

			mTweenRot2.duration=rotaterDulation;

		}
		public void Show(int width,string labelString){
			//カバーで覆う
			this.mPanel.depth=width+1;

			mCollider.enabled=true;
			mLb.enabled=true;
			NGUITools.SetActive(mTweenRot1.gameObject,true);

			mTweenRot1.duration=rotaterDulation;
			NGUITools.SetActive(mTweenRot2.gameObject,true);

			mTweenRot2.duration=rotaterDulation;
			mLb.text=labelString;
			
		}
		public void Hide(){
			mCollider.enabled=false;
			mLb.enabled=false;
			NGUITools.SetActive(mTweenRot1.gameObject,false);
			NGUITools.SetActive(mTweenRot2.gameObject,false);
		}

		#endregion

	}

}