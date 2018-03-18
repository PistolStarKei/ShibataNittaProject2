using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// MapOptimizerの説明
/// </summary>
public class MapOptimizer : MonoBehaviour {


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
				BoxCollider bc=child.gameObject.GetComponent<BoxCollider>();
				if(bc!=null){
					bc.center=new Vector3(0f,0f,-0.5f);
				}
			}else{
				if (child.name == "bounds") {
					if(child.transform.localPosition==Vector3.zero){
						Object.Destroy(child.gameObject); 
					}else{
						child.parent = this.transform;
						BoxCollider bc=child.gameObject.GetComponent<BoxCollider>();
						if(bc==null){
							BoxCollider col=child.gameObject.AddComponent<BoxCollider>();
							col.size= new Vector3(.5f,.5f,.5f);
						}
					}

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
							if(child.gameObject!=this.gameObject){
								Destroy(child.gameObject);
							}
						}
					}
				}
			}
		}




	}
	#endregion




}
