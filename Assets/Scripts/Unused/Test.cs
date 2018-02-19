using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using PS_Util;
using System.Linq;

public class Test :MonoBehaviour {
	

	void Start () {
		Debug.Log(""+GetRandomPU_Subweapon());

	}

	Pickup GetRandomPU_Kaifuku(){
		int rand=UnityEngine.Random.Range(0,101);

		var sorted = PSParams.SpawnItemRates.Rate_Kaifuku.OrderBy((x) => x.Value);  

		int sum=0;
		foreach(var s in sorted){
			sum+=s.Value;
			Debug.Log(sum.ToString()+" "+s.Value);
			if(rand<=sum){
				return s.Key;
			}
		}
		return Pickup.CureS;
	}

	Pickup GetRandomPU_Subweapon(){
		int rand=UnityEngine.Random.Range(0,101);
		var sorted = PSParams.SpawnItemRates.Rate_Subweapon.OrderBy((x) => x.Value);  

		int sum=0;
		foreach(var s in sorted){
			sum+=s.Value;
			Debug.Log(sum.ToString()+" "+s.Value);
			if(rand<=sum){
				return s.Key;
			}
		}

		return Pickup.NAPAM;
	}

}
