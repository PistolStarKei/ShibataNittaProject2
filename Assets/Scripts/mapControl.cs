using UnityEngine;
using System.Collections;

public class mapControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		changeLayerNameToWall ();
	}

	// マップブロックで空白以外はWallに変えて当たりをつける
	public void changeLayerNameToWall (){
		GameObject[] emptyBlocks = GameObject.FindGameObjectsWithTag ("EmptyBlock");

		foreach(GameObject block in emptyBlocks) {
			block.layer = LayerMask.NameToLayer ("Untagged");	//名前から番号[int]
		}

		GameObject[] emptyCubes = GameObject.FindGameObjectsWithTag ("EmptyCube");

		foreach(GameObject block in emptyCubes) {
			block.layer = LayerMask.NameToLayer ("Untagged");	//名前から番号[int]
		}
	}

	void start(){
		changeLayerNameToWall();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
