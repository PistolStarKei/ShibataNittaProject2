using UnityEngine;
using System.Collections;

public class Razer : MonoBehaviour {


	public Transform startPoint;
	public Transform targetPoint;

	public LineRenderer line;
	// Update is called once per frame
	void Update () {
		line.SetPosition(0,startPoint.position);
		line.SetPosition(1,targetPoint.position);
	}
}
