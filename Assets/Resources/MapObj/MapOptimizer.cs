using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MapOptimizerの説明
/// </summary>
public class MapOptimizer : MonoBehaviour {

	#region  メンバ変数

	#endregion

	#region  初期化

	void Awake () {
	}

	void Start () {
		Transform[] transformArray = this.GetComponentsInChildren<Transform> ();

		foreach (Transform child in transformArray) {
			if (child.name == "sfcrate") {
				child.parent = this.transform;
				child.gameObject.isStatic=true;
				OrientedBlock sc=child.gameObject.GetComponent<OrientedBlock>();
				if(sc!=null){
					Object.Destroy(sc); 
				}
			}else{
				if (child.name == "bounds") {
					child.parent = this.transform;
					/*OrientedBlock sc=child.gameObject.GetComponent<OrientedBlock>();
					if(sc==null){
						Destroy(child.gameObject);
					}else{
						Object.Destroy(sc); 
					}*/
				}else{
					if (child.name == "SpawnPoint") {
						child.parent = this.transform;
						OrientedBlock sc=child.gameObject.GetComponent<OrientedBlock>();
						if(sc==null){
							Destroy(child.gameObject);
						}else{
							Object.Destroy(sc); 
						}
						BoxCollider box=child.gameObject.GetComponent<BoxCollider>();
						if(box!=null){
							Object.Destroy(box); 
						}

					}else{
						if (child.name == "SFCrate") {
							child.parent = this.transform;
							child.gameObject.isStatic=true;
						}else{
							if(child.gameObject!=this.gameObject && child.name!="SFCrate"){
								Destroy(child.gameObject);
							}
						}
					}
				}
			}


		}
	}
	#endregion


	#region  Update
	
	void Update(){
	
	}

	#endregion


	


	#region  Public関数
	

	#endregion
	

	#region  メンバ関数
	
	#endregion
}
