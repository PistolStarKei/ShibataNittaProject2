using UnityEngine;

public class TapEffect : MonoBehaviour
{
	[SerializeField] ParticleSystem tapEffect;              // タップエフェクト

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			// マウスのワールド座標までパーティクルを移動し、パーティクルエフェクトを1つ生成する
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 5.0f;
			Vector3 tmpPos2 = Camera.main.ScreenToWorldPoint (mousePos);

			//			var pos = _camera.ScreenToWorldPoint(Input.mousePosition + _camera.transform.forward * 100);
			tapEffect.transform.position = tmpPos2;
			tapEffect.Emit(1);
		}
	}
}
