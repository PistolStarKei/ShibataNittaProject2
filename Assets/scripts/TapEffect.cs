using UnityEngine;

public class TapEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem tapEffect;              // タップエフェクト
	public static Vector3 tapPos;
	public static bool tapFlag = false;

	void Update()
	{
		tapFlag = false;
		var phase = GodTouches.GodTouch.GetPhase ();
		if (phase == GodTouches.GodPhase.Began) {

			getTapPos ();

			// マウスのワールド座標までパーティクルを移動し、パーティクルエフェクトを1つ生成する
			tapEffect.transform.position = tapPos;
			tapEffect.Emit (1);
			//			Debug.Log("tmpPos == " + tapPos , this);
			tapFlag = true;

		} else if (phase == GodTouches.GodPhase.Moved) {
			getTapPos ();
			tapFlag = true;

		} else if (phase == GodTouches.GodPhase.Ended) {
			
		}
	}

	private void getTapPos(){
		Vector3 mousePos = GodTouches.GodTouch.GetPosition ();
		mousePos.z = 5.0f;
		tapPos = Camera.main.ScreenToWorldPoint (mousePos);
	}

}
