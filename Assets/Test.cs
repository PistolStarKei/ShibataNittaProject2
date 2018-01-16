using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Test : MonoBehaviour {


	// Use this for initialization
	void Update () {
		
		Debug.Log(""+Camera.main.WorldToViewportPoint(transform.position));
	}

	Vector3 vec;
	bool IsInsideView(Vector3 position){
		vec=Camera.main.WorldToViewportPoint(position);
		if(vec.x<0.0f)return false;
		if(vec.y<0.0f)return false;
		if(vec.x>1.0f)return false;
		if(vec.y>1.0f)return false;
		return true;

	}


}
