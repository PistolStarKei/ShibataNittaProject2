using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Test :MonoBehaviour {
	
	Dictionary<string, int[]> ArrayLst = new Dictionary<string, int[]>();
	int[] a = new int[] {1,2,3,4};
	int[] b = new int[] {4,3,2,1,0};



	void Start () {
		ArrayLst.Add("a", a);
		ArrayLst.Add("b", b);

		string ArrayName = "a";
		int[] c = null;
		if (ArrayLst.ContainsKey(ArrayName))
		{
			c = ArrayLst[ArrayName];
		}

		Debug.Log("Variable value: " + c.Length);
	
	}

}
