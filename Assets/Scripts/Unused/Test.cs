using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using PS_Util;
using System.Linq;

public class Test :MonoBehaviour {
	
	public GameObject prehab;
	public RandomPointGenerater rpg;
	void Start () {
		for(int i=0;i<4;i++){
			Vector3 pos=rpg.GetRandomPoint(0,i);
			pos.y=0.0f;
			GameObject go=Instantiate(prehab,pos,Quaternion.identity,null) as GameObject;
			go.name=i.ToString();

		}

	}



}
