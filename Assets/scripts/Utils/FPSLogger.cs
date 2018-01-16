using UnityEngine;
using System.Collections;

namespace PS_Util{
	
	public class FPSLogger : MonoBehaviour {

		public UILabel lb;
		void SetText(string tx){
			lb.text=tx;
		}

		float deltaTime = 0.0f;


		// Update is called once per frame
		void Update () {
			deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;
			SetText(string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps));
		}
	}
}
