using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;

public class GetItemEffect : MonoBehaviour {



	#region  Public変数
	[Header("参照用")]
	public TweenPosition _tweenPosition;
	public UISprite _sprite;

	[Header("Public変数")]
	public float _moveDulation=0.5f;
	#endregion

	#region  メンバ変数
	Subweapon _weapon=Subweapon.NAPAM;
	#endregion

	#region  初期化
	void Awake () {
		_tweenPosition=gameObject.GetComponent<TweenPosition>();
		_sprite=gameObject.GetComponent<UISprite>();

	}
	#endregion

	#region  Public関数
	//スポーン後に呼び出す
	public void OnInit(Subweapon weapon,string spriteName){
		this._weapon=weapon;

		//ここでスプライトの画像を設定する
		_sprite.spriteName=spriteName;

		//自機の位置（world position）を取得する

		Vector3 shipPosition=GUIManager.Instance.shipControll.transform.position;
		//スロットの位置（world positionを取得する
		Vector3 slotPosition=GUIManager.Instance.subWeaponSlot.currentSub.transform.position;

		//自機の位置をこのUIオブジェクトのローカルポジションに変換する
		_sprite.transform.OverlayPosition(GUIManager.Instance.shipControll.transform);
		shipPosition=transform.localPosition;

		//スロットの位置をこのUIオブジェクトのローカルポジションに変換する
		slotPosition=transform.InverseTransformPoint(slotPosition);
		slotPosition.z=0f;
		StartMove(shipPosition,slotPosition,_moveDulation);
		OnInstantiated();
	}
	#endregion

	#region  イベント
	//スポーン時に呼び出される
	void OnInstantiated(){
		
	}

	//移動完了後に呼び出される
	void OnMoved(){
		//移動完了　ここでサブウェポンをスロットに追加する
		GUIManager.Instance.subWeaponSlot.AddSubWeaponToHolder(this._weapon);

		//移動完了　オブジェクトを消去する
		KillSelf();
	}
	#endregion


	#region  ボタンなどの受け取りイベント
	public void OnMovedEvent(){
		if(_tweenPosition.direction==AnimationOrTween.Direction.Forward){
			OnMoved();
		}
	}
	#endregion



	#region  メンバ関数

	void KillSelf(){
		PoolManager.Pools["Particles"].Despawn(gameObject.transform);
	}

	void StartMove(Vector3 from,Vector3 to,float dulation){
		_tweenPosition.from=from;
		_tweenPosition.to=to;
		_tweenPosition.duration=dulation;
		_tweenPosition.ResetToBeginning();
		_tweenPosition.PlayForward();
	}
	#endregion
}
