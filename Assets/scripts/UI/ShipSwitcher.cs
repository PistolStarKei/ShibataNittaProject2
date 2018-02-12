using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipSwitcher : PS_SingletonBehaviour<ShipSwitcher> {

	public List<GameObject> ships;
	public GUI_ShipRotater currentShip;

	public Vector3[] pos;
	public Vector3 currentTrans;

	public float speed=0.0f;

	// Use this for initialization
	void Awake () {
		foreach (Transform child in transform){
			ships.Add(child.gameObject);
		}
	}

	public AnimationCurve curve;
	public bool speedByCurve=false;
	float lerpVal=0.0f;
	float startTime=0.0f;
	void Update(){
		
		startTime+=Time.deltaTime;
		if(speedByCurve){
			lerpVal+=curve.Evaluate(startTime)*startTime;
		}else{
			lerpVal+=speed*startTime;
		}


		transform.position=Vector3.Lerp(transform.position,currentTrans,lerpVal);
	}
	
	public void Set(int index){
		//ClearAll();
		//ships[index].SetActive(true);
		currentShip=ships[index].GetComponent<GUI_ShipRotater>();
		currentShip.SetToDefault();
		currentTrans=pos[index];
		lerpVal=0.0f;
		startTime=0.0f;
	}

	void ClearAll(){
		foreach (GameObject go in ships){
			go.SetActive(false);
		}
	}



	public void SetColor(int shipNum,int colorNum){
		string path="ShipMaterials/Ship"+(shipNum+1)+"/"+"Ship"+(shipNum+1)+"c"+(colorNum+1);
		StartCoroutine(LoadMaterialAndSet(shipNum,path));

	}

	public Material currentMat;
	public IEnumerator LoadMaterialAndSet (int shipNum,string filePath)
	{
		// リソースの非同期読込開始
		PSPhoton.LobbyManager.instance.cover.Cover();
		ResourceRequest resReq = Resources.LoadAsync<Material> (filePath);
		// 終わるまで待つ
		while (resReq.isDone == false) {
			yield return 0;
		}
		// テクスチャ表示
		currentMat=resReq.asset as Material;
		Material[] mats =ships[shipNum].GetComponent<MeshRenderer>().materials;
		mats[ships[shipNum].GetComponent<GUI_ShipRotater>().changeMat] = currentMat;

		ships[shipNum].GetComponent<MeshRenderer>().materials=mats;

		PSPhoton.LobbyManager.instance.cover.Uncover();
	}


}
