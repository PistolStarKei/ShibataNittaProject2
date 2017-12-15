using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float scale = Random.Range (0.2f, 4.0f);
		transform.localScale = new Vector3(scale, scale, scale);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime * 0.1f, Space.World);
	}
}
