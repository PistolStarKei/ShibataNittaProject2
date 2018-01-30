using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 雷のエフェクトを発生させる
/// </summary>
public class LightningEffect : MonoBehaviour {

	#region  メンバ変数

	public ParticleSystem[] mLightningLists;

	#endregion

	#region  初期化

	void Start () {
		HideLightnings();
	}
	#endregion

	#region  Public関数
	public void ShowLightnings(){
		//エフェクトループをランダムに揺らがせる
		foreach(ParticleSystem ps in mLightningLists){
			ps.startDelay=Random.Range(0.0f,0.5f);
			ps.gameObject.SetActive(true);
		}
	}

	public void HideLightnings(){
		foreach(ParticleSystem ps in mLightningLists){
			ps.gameObject.SetActive(false);
		}
	}

	#endregion

}
