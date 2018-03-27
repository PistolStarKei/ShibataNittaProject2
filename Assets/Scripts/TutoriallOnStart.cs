using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutoriallOnStart : MonoBehaviour {

	[System.Serializable]
	public struct UIObjectData{
		public int depth;
		public int layer;
		public Transform parent;
		public Vector3 localPosition;
		public List<EventDelegate> prevDelegate;
	}
	#region  Public変数
	public int _TutorialProgress=0;
	public UILabel _textLb;
	public UIObjectData _CashedData;
	public UISprite _FlagSp; 
	public Transform _Indecater;

	public GameObject _bg;
	public FlagSelecter _flagSelecter;

	public GameObject _NameInputs;
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
		_TutorialProgress=0;
		Next();
		//Flagをハイライトする
		//文字列を出す　「まずは国旗を選択しよう」
	}
	#endregion

	#region  Update
	void Update () {
	
	}
	#endregion

	#region  Public関数

	#endregion

	#region  ボタンなどの受け取りイベント
	public void OnClickFlag(){
		Debug.Log("OnClickFlag");
		//ここで国旗セレクト画面をだす。
		NGUITools.SetActive(_bg,false);
		NGUITools.SetActive(_textLb.gameObject,false);
		NGUITools.SetActive(_Indecater.gameObject,false);
		_flagSelecter.ShowWithCallback(OnCloseFlagSelect);

	}
	public void OnCloseFlagSelect(){
		NGUITools.SetActive(_bg,true);

		Next();
	}

	UIInput _input;
	public void OnEnterName(){
		Debug.Log("OnEnterName"+_input.value);
		if(_input.value==""){
			_textLb.text=Localization.Get("AlertNullInput");
			return;

		}

		if(_input.value=="タップで名前を入力"|| _input.value=="Tap here to input"){
			_textLb.text=Localization.Get("AlertNoInput");
			return;
		}



		_input.onSubmit=_CashedData.prevDelegate;
		_NameInputs.gameObject.layer=_CashedData.layer;
		_NameInputs.gameObject.transform.parent=_CashedData.parent;
		_NameInputs.transform.localPosition=_CashedData.localPosition;
		_NameInputs.gameObject.GetComponent<UIWidget>().depth=_CashedData.depth;


		foreach (Transform trans in _NameInputs.transform)
		{
			trans.gameObject.layer = this.gameObject.layer;
		}

		NGUITools.MarkParentAsChanged(_NameInputs.gameObject);



		DataManager.Instance.gameData.username=_input.value;
		DataManager.Instance.SaveAll();
		OnFinished();
	}

	#endregion

	#region  イベント

	void OnFinished(){
		//tweetInfo.Show();
		DataManager.Instance.envData.isTutorialed=true;
		DataManager.Instance.SaveAll();
		KillSelf();
	}
	#endregion

	#region  メンバ関数
	void Next(){
		switch(_TutorialProgress){
			case 0:
				_CashedData=new UIObjectData();
				_CashedData.depth=_FlagSp.depth;
				_CashedData.parent=_FlagSp.transform.parent;
				_CashedData.localPosition=_FlagSp.transform.localPosition;
				_CashedData.prevDelegate=_FlagSp.transform.gameObject.GetComponent<UIEventTrigger>().onClick;
				_CashedData.layer=_FlagSp.transform.gameObject.layer;
				
				_FlagSp.transform.parent=this.transform;
				_FlagSp.transform.gameObject.layer=this.gameObject.layer;
				
				List<EventDelegate> delegateTemp=new List<EventDelegate>();
				EventDelegate del=new EventDelegate(this,"OnClickFlag");
				delegateTemp.Add(del);
				_FlagSp.transform.gameObject.GetComponent<UIEventTrigger>().onClick=delegateTemp;
				TweenPosition tp=_Indecater.gameObject.GetComponent<TweenPosition>();
					tp.from=new Vector3(-486.91f,tp.from.y,tp.from.z);
					tp.to=new Vector3(-486.91f,tp.to.y,tp.to.z);
				_textLb.text=Localization.Get("Desc_ServerSelect");
				break;
			case 1:
				_FlagSp.transform.parent=_CashedData.parent;
				_FlagSp.depth=_CashedData.depth;
				_FlagSp.transform.localPosition=_CashedData.localPosition;
				_FlagSp.transform.gameObject.layer=_CashedData.layer;
				_FlagSp.transform.gameObject.GetComponent<UIEventTrigger>().onClick=_CashedData.prevDelegate;
				
				NGUITools.MarkParentAsChanged(_FlagSp.gameObject);

				_CashedData=new UIObjectData();
				_CashedData.layer=_NameInputs.layer;
				_CashedData.parent=_NameInputs.transform.parent;
				_CashedData.localPosition=_NameInputs.transform.localPosition;
				_CashedData.depth=_NameInputs.gameObject.GetComponent<UIWidget>().depth;
				_input=_NameInputs.gameObject.GetComponent<UIInput>();
				_CashedData.prevDelegate=_input.onSubmit;
				

				List<EventDelegate> delegateTemp2=new List<EventDelegate>();
				EventDelegate del2=new EventDelegate(this,"OnEnterName");
				delegateTemp2.Add(del2);
				
				_input.onSubmit=delegateTemp2;
				_NameInputs.gameObject.layer=this.gameObject.layer;
				_NameInputs.gameObject.transform.parent=this.transform;
				
				foreach (Transform trans in _NameInputs.transform)
				{
					trans.gameObject.layer = this.gameObject.layer;
				}
				
				NGUITools.MarkParentAsChanged(_NameInputs.gameObject);
				
				_textLb.text=Localization.Get("Desc_SetName");
				NGUITools.SetActive(_textLb.gameObject,true);

				TweenPosition tp2=_Indecater.gameObject.GetComponent<TweenPosition>();

				tp2.from=new Vector3(-252.76f,tp2.from.y,tp2.from.z);
				tp2.to=new Vector3(-252.76f,tp2.to.y,tp2.to.z);
				
				
				NGUITools.SetActive(_Indecater.gameObject,true);
				break;
		}


		gameObject.GetComponent<UIPanel>().RebuildAllDrawCalls();

		_TutorialProgress++;

	}


	void KillSelf(){
		Destroy(gameObject);
	}
	#endregion
}
