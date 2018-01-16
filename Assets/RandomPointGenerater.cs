using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PS_Util{
	
	public class RandomPointGenerater : MonoBehaviour {

		public List<PS_Util.RandomPointBounds> randomBounds=new List<PS_Util.RandomPointBounds>();

		// Use this for initialization
		void Awake () {
			foreach(Transform tr in gameObject.transform){

				PS_Util.RandomPointBounds bounds=tr.gameObject.GetComponent<PS_Util.RandomPointBounds>();
				if(bounds){
					randomBounds.Add(bounds);
				}
			}
		}

		public Vector3 GetRandomPoint(){
			if(randomBounds.Count<=0)return Vector3.zero;
			return randomBounds[Random.Range(0,randomBounds.Count)].GetRandomPointInArea();

		}
	}
}
