using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// StartCountDownの説明
/// </summary>
public class StartCountDown : MonoBehaviour {

	#region  メンバ変数
	void Start(){
		//StartCoroutine(InVoker());
	}
	#endregion

	int num=3;
	IEnumerator InVoker(){
		while(num>=0){
			yield return new WaitForSeconds(1.0f);
			SetCount(num);

			num--;
		}
	}

	public UITexture tex1;
	public UITexture tex2;
	public TweenPosition tp1;
	public TweenPosition tp2;
	public TweenAlpha ta1;
	public TweenAlpha ta2;
	public TweenScale ts1;
	public TweenScale ts2;
	public Texture2D[] numbers;

	int count=100;
	#region  Public関数
	public void SetCount(int num){
		if(num<0 || num>3)return;
		if(count!=num){
			SetTexture(num);
			switch(num){
				case 3:
					tp1.PlayForward();
					tp2.PlayForward();
					ta1.PlayForward();
					ta2.PlayForward();
					break;
				case 2:
					tp1.ResetToBeginning();
					tp2.ResetToBeginning();
					ta1.ResetToBeginning();
					ta2.ResetToBeginning();
					tp1.PlayForward();
					tp2.PlayForward();
					ta1.PlayForward();
					ta2.PlayForward();
					break;
				case 1:
					tp1.ResetToBeginning();
					tp2.ResetToBeginning();
					ta1.ResetToBeginning();
					ta2.ResetToBeginning();
					tp1.PlayForward();
					tp2.PlayForward();
					ta1.PlayForward();
					ta2.PlayForward();
					break;
				case 0:
					Hide();
					break;
			}
			count=num;
		}
	}

	public void OnTweened(){
		if(ts1.direction==AnimationOrTween.Direction.Forward){
			KillSelf();
		}
	}

	#endregion
	

	#region  メンバ関数

	void Hide(){
		ta1.duration=1.0f;
		ta2.duration=1.0f;
		ta1.PlayReverse();
		ta2.PlayReverse();
		ts1.PlayForward();
		ts2.PlayForward();
	}
	void SetTexture(int num){
		Debug.Log("SetTexture"+num);
		tex1.mainTexture=numbers[num];
		tex2.mainTexture=numbers[num];
	}
	void KillSelf(){
		Destroy(this.gameObject);
	}
		
	#endregion
}
