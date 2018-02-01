using UnityEngine;
using System.Collections;

public class mapControl : MonoBehaviour {

	public int mapNumber = 0;

	public void setMapNumber(int num){
		mapNumber = num;
	}

	// Use this for initialization
	void Start () {
//		attachMapData ();
		setMapBlockTag ();
		changeLayerNameToWall ();
	}

	public void attachMapData(){
		//mapNumber;

		// mapManagerの子供にmapDataをつける処理
		GameObject mapManager = GameObject.Find ("mapManager");
//		string str = sprintf ("stage%02d", mapNumber);

	}

	// MapChunk以下のmapオブジェクトすべてにMapBlockタグをつける
	public void setMapBlockTag (){

		GameObject mapManager = GameObject.Find ("mapManager");

		Transform[] transformArray = mapManager.GetComponentsInChildren<Transform>();
		foreach (Transform child in transformArray)
		{
			child.tag = "MapBlock";
		}
	}

	// マップブロックで空白以外はWallに変えて当たりをつける
	public void changeLayerNameToWall (){

		GameObject[] emptyBlocks = GameObject.FindGameObjectsWithTag ("MapBlock");
		Debug.Log (emptyBlocks.Length);
		foreach(GameObject block in emptyBlocks) {
			var scr = block.GetComponent<OrientedBlock>();
			if (scr != null) {
				BlockOrientation num = scr.GetOrientation ();
				if ( num != BlockOrientation.Empty ) {
					block.layer = LayerMask.NameToLayer ("Wall");
				}
			}
		}

//		GameObject[] emptyBlocks = GameObject.FindGameObjectsWithTag ("EmptyBlock");
//		foreach(GameObject block in emptyBlocks) {
//			block.layer = LayerMask.NameToLayer ("Untagged");	//名前から番号[int]
//		}
//
//		GameObject[] emptyCubes = GameObject.FindGameObjectsWithTag ("EmptyCube");
//
//		foreach(GameObject block in emptyCubes) {
//			block.layer = LayerMask.NameToLayer ("Untagged");	//名前から番号[int]
//		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
