using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PS_Util{
	
	public class RandomPointGenerater : MonoBehaviour {

		public List<PS_Util.RandomPointBounds> randomBounds=new List<PS_Util.RandomPointBounds>();
		bool isInited=false;
		// Use this for initialization
		void Start () {
			if(!isInited)Init();
		}


		void Init(){
			foreach(Transform tr in gameObject.transform){

				PS_Util.RandomPointBounds bounds=tr.gameObject.GetComponent<PS_Util.RandomPointBounds>();
				if(bounds){
					randomBounds.Add(bounds);
				}
			}
		}
		public Vector3 GetRandomPoint(){
			if(!isInited)Init();
			if(randomBounds.Count<=0)return Vector3.zero;
			return randomBounds[Random.Range(0,randomBounds.Count)].GetRandomPointInArea();

		}
	}
}
