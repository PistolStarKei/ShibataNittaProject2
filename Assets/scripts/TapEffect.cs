using UnityEngine;

public class TapEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem tapEffect;              // タップエフェクト
//	[SerializeField] Camera _camera;                        // カメラの座標

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			// マウスのワールド座標までパーティクルを移動し、パーティクルエフェクトを1つ生成する
			Vector3 mousePos = GodTouches.GodTouch.GetPosition ();
			mousePos.z = 5.0f;
			Vector3 tmpPos2 = Camera.main.ScreenToWorldPoint (mousePos);

//			var pos = _camera.ScreenToWorldPoint(Input.mousePosition + _camera.transform.forward * 100);
			tapEffect.transform.position = tmpPos2;
			tapEffect.Emit(1);
//			Debug.Log("tmpPos == " + tmpPos2 , this);
		}
	}
}
