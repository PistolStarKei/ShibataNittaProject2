using UnityEngine;
using System.Collections;

public class mapControl : MonoBehaviour {

	public int mapNumber = 0;
	public GameObject stage00;
	public GameObject stage01;
	public GameObject stage02;
	public GameObject stage03;

	public void setMapNumber(int num){
		mapNumber = num;
	}

	// Use this for initialization
	void Start () {
		attachMapData ();
	}

	public void attachMapData(){
		// mapManagerの子供にmapDataをつける処理

		//自分のインスタンスなので不要です。
		//GameObject mapManager = GameObject.Find ("mapManager");

		switch (mapNumber) {
		case 0:
			GameObject go=Instantiate (stage00);

			//これはInstantiateされたオブジェクトではなくて、プレハブに親をつけている
			//stage00.transform.parent = mapManager.transform;

			//Instantiateされたオブジェクトに、自分のスクリプトの付いているGOのtransformを親に設定する
			go.transform.parent = this.gameObject.transform;
			break;
		case 1:
			Instantiate (stage01);
			break;
		case 2:
			Instantiate (stage02);
			break;
		case 3:
			Instantiate (stage03);
			break;
		}
	}
}