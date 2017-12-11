using UnityEngine;
using System.Collections;

namespace GodTouches{
	public class touch : MonoBehaviour {

		public float delta = 0.01f;
		public float speed = 3;
		public Transform Move;
		Vector3 startPos;

		// Use this for initialization
		void Start () {
			startPos = Move.position;
		}

		// Update is called once per frame
		void Update () {
			// タッチを検出して動かす
//			var phase = GodTouch.GetPhase ();
//			if (phase == GodPhase.Began) 
//			{
//				startPos = Move.position;
//			}
//			else if (phase == GodPhase.Moved) 
//			{
//				Vector3 temp;
//				temp.x = GodTouch.GetPosition ().x * delta - (Screen.currentResolution.width / 1000);
//				temp.y = 0;
//				temp.z = GodTouch.GetPosition ().y * delta - (Screen.currentResolution.height / 1000);
//				Move.position = temp;
//				//              Move.position += GodTouch.GetDeltaPosition(); 
//			}
//			else if (phase == GodPhase.Ended) 
//			{
//				Move.position = startPos;
//			}

//			var tmpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//			Debug.Log("tmpPos == " + tmpPos , this);

			var phase = GodTouch.GetPhase ();
			if (phase == GodPhase.Began) {
				startPos = Move.position;

			} else if (phase == GodPhase.Moved) {
				Vector3 mousePos = GodTouch.GetPosition ();
				mousePos.z = 5.0f;
				Vector3 tmpPos2 = Camera.main.ScreenToWorldPoint (mousePos);
				Move.position = tmpPos2;
//				Debug.Log("tmpPos == " + tmpPos2 , this);

			} else if (phase == GodPhase.Ended) 
			{
				Move.position = startPos;
			}

		}
	}
}
