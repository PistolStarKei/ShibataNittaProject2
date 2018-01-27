using UnityEngine;
using System.Collections;
using Colorful;

public class AnalogTVAnimator : MonoBehaviour {

	public AnalogTV tv;

	float offset=0.0f;
	public float speed=1.0f;
	// Update is called once per frame
	void FixedUpdate () {
		tv.ScanlinesOffset+=offset+(Time.deltaTime*speed);
		if(tv.ScanlinesOffset>50.0f){
			tv.ScanlinesOffset=0.0f;
		}
	}
}
