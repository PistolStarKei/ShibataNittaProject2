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

	// mapManagerの子供にmapDataをつける処理
	public void attachMapData(){
		switch (mapNumber) {
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
		}

		// スポーン位置とアイテムの位置を確定
		itemManager im = GetComponent<itemManager>();
		im.checkSpawnPrefab ();
		im.checkItemPrefab ();
	}
}