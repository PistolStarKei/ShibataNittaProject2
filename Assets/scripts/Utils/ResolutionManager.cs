using UnityEngine;
using System.Collections;

public class ResolutionManager : MonoBehaviour {

	public float targetResolution=1024.0f;

	// Use this for initialization

	//もしも縦長なら、Screen.Heightに変更する
	void Awake () {
		float screenRate = targetResolution / Screen.width;
		if( screenRate > 1 ) screenRate = 1;
		int width = (int)(Screen.width * screenRate);
		int height = (int)(Screen.height * screenRate);

		Screen.SetResolution( width , height, true, 15);
	}
}
