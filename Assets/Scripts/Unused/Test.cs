using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using PS_Util;

public class Test :MonoBehaviour {
	
	public GameObject go;
	public RandomPointGenerater point;

	void Start () {
		StartCoroutine( Emitter ());
	}
	IEnumerator Emitter () {
		while(true){
			GameObject.Instantiate(go,point.GetRandomPoint(),Quaternion.identity);
			yield return new WaitForSeconds(.5f);
		}
	}
}
