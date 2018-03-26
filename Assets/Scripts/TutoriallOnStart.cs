using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutoriallOnStart : MonoBehaviour {

	struct UIObjectData{
		public int depth;
		public Transform parent;
		public Vector3 localPosition;
		public List<EventDelegate> prevDelegate;
	}
	#region  Public変数

	#endregion

	#region  メンバ変数
	
	#endregion

	#region  初期化
	void Start () {
		//ここでチュートリアルの必要性を加味する
		if(DataManager.Instance.envData.isTutorialed){
			KillSelf();
			return;
		}


	}
	#endregion

	#region  Update
	void Update () {
	
	}
	#endregion

	#region  Public関数

	#endregion

	#region  ボタンなどの受け取りイベント

	#endregion

	#region  イベント

	void OnFinished(){
		//tweetInfo.Show();
		KillSelf();
	}
	#endregion

	#region  メンバ関数
	void KillSelf(){
		Destroy(gameObject);
	}
	#endregion
}
