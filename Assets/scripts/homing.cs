using UnityEngine;
using System.Collections;

public class homing : MonoBehaviour {

	private GameObject mainTarget;

	void Start () {
		// 索敵してターゲットを決める
		// 一番近い敵をターゲットにする
		GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("enemy");

//		foreach (GameObject obj in tagobjs) {
//			
//		}

		int targetNo=-1;
		float minDis=9999f;
		float dis=0f;

		for(int i=0;i<=tagobjs.Length;i++)
		{
			GameObject target = tagobjs[i];
			if(target == null ) continue;
			dis = Vector3.Distance(transform.position,target.transform.position);
			if(dis<minDis) { minDis=dis; targetNo=i; }
		}

		mainTarget = tagobjs [targetNo];
	}

	float speed = 1.0f;

	void Update () {
		if (mainTarget != null) {
			float step = Time.deltaTime * speed;
			transform.position = Vector3.MoveTowards(transform.position, mainTarget.transform.position, step);
		}
	}
}
