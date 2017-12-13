using UnityEngine;
using System.Collections;

/// スカイボックスを回転させる
/// 【使い方】
/// 《前提》SkyboxCamera（スカイボックス専用カメラを設置していること）
/// ①シーンのどのオブジェクトでもいいのでSkyboxRotationをアサイン
/// ②インスペクターでskyboxCameraへSkyboxCameraをアサイン（スカイボックス専用カメラをアサイン）
/// ③インスペクターでskyboxChangeAngleとskyboxAxisを設定
public class SkyboxRotation : MonoBehaviour {

	public GameObject skyboxCamera;			//スカイボックスカメラ
	public float skyboxChangeAngle = 0.02f;	//１フレームで回転させたい値
	public Vector3 skyboxAxis = new Vector3 (-1, 0, 0);	//回転軸
	float skyboxAngle;						//変更後の角度（アングル）

	//スカイボックスを回転させる
	void Update ()
	{
		skyboxAngle += skyboxChangeAngle;
		skyboxCamera.transform.rotation = Quaternion.AngleAxis(skyboxAngle, skyboxAxis);
	}
}
