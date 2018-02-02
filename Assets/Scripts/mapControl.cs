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
		GameObject mapManager = GameObject.Find ("mapManager");

		switch (mapNumber) {
		case 0:
			Instantiate (stage00);
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