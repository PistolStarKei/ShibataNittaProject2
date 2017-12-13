using UnityEngine;
using System.Collections;

public class skyBox : MonoBehaviour {
	public float angle = 0.1f;
	float rotate = 0.0f;

	void Start() {
	}
	void Update () {
		//rotateにangleを足していく
		rotate += angle;
		//もしrotateが360以上になったら_rotから-360引いてループさせる
		if (rotate >= 360.0f){
			rotate -= 360.0f;
		}
		//skyboxのrotateを回す関数
		RenderSettings.skybox.SetFloat("_Rotation", rotate);
		Debug.Log("tmpPos == " + rotate , this);
	}
}
