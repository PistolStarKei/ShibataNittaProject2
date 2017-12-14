using UnityEngine;

public class TapEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem tapEffect;              // タップエフェクト
	public static Vector3 tapPos;
	public static bool tapFlag = false;

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			// マウスのワールド座標までパーティクルを移動し、パーティクルエフェクトを1つ生成する
			Vector3 mousePos = GodTouches.GodTouch.GetPosition ();
			mousePos.z = 5.0f;
			tapPos = Camera.main.ScreenToWorldPoint (mousePos);

			tapEffect.transform.position = tapPos;
			tapEffect.Emit (1);
//			Debug.Log("tmpPos == " + tapPos , this);
			tapFlag = true;
		} else
			tapFlag = false;
	}
}
