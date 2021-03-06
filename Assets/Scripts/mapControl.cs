﻿using UnityEngine;
using System.Collections;
using PS_Util;

public class mapControl : MonoBehaviour {

	public int mapNumber = 0;

	/*public GameObject stage00;
	public GameObject stage01;
	public GameObject stage02;
	public GameObject stage03;*/

	public void setMapNumber(int num){
		mapNumber = num;
	}

	public bool loadLevelOnStart=false;

	// Use this for initialization
	void Start () {
		if(loadLevelOnStart)attachMapData ();

	}

	// mapManagerの子供にmapDataをつける処理
	public void attachMapData(){

		GameObject go = Resources.Load("MapObj/stage0"+mapNumber.ToString()) as GameObject;
		GameObject go2 = Instantiate (go) as GameObject;
		go2.transform.parent = this.gameObject.transform;

		/*switch (mapNumber) {
		case 0:
			{
				//Instantiateされたオブジェクトに、自分のスクリプトの付いているGOのtransformを親に設定する
				GameObject go = Instantiate (stage00);
				go.transform.parent = this.gameObject.transform;

				break;
			}
		case 1:
			{
				GameObject go = Instantiate (stage01);
				go.transform.parent = this.gameObject.transform;
				break;
			}
		case 2:
			{
				GameObject go = Instantiate (stage02);
				go.transform.parent = this.gameObject.transform;
				break;
			}
		case 3:
			{
				GameObject go = Instantiate (stage03);
				go.transform.parent = this.gameObject.transform;
				break;
			}
		}*/

		// スポーン位置とアイテムの位置を確定
		checkSpawnPrefab ();
		checkItemPrefab ();
	}

	public Transform mSpawnPointsParent;
	public Transform mSpawnBoundsParent;

	// スポーンポイントになるプレハブがあるか調べる
	private void checkSpawnPrefab(){


		//GameObject mapManager = GameObject.Find ("mapManager");
		Transform[] transformArray = this.GetComponentsInChildren<Transform> ();

		foreach (Transform child in transformArray) {
			if (child.name == "SpawnPoint") {
				child.parent = mSpawnPointsParent;
			}
		}
	}

	// アイテムポイントになるプレハブがあるか調べる
	private void checkItemPrefab(){

		Transform[] transformArray = this.GetComponentsInChildren<Transform> ();

		foreach (Transform child in transformArray) {
			if (child.name == "bounds") {
				child.parent = mSpawnBoundsParent;
				if(child.GetComponent<RandomPointBoundsBox>()==null){
					child.gameObject.AddComponent<RandomPointBoundsBox>();
				}
			}
		}
	}

}