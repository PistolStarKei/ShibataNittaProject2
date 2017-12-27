using UnityEngine;
using System.Collections;

public class PS_GUI_Tapdetecter : MonoBehaviour {


	public Camera uiCam;
	public Camera mainCam;

	public bool isActive=true;

	public Transform tapIndecator;

	void Start(){
		tapIndecator.gameObject.SetActive(false);
	}

	void SetUIPositon(Vector3 tapPos,Transform uiElement){
		Transform parent = uiElement.parent;
		uiElement.localPosition = (parent != null) ? parent.InverseTransformPoint(tapPos) : tapPos;
	}

	void OnPress(bool isPress){
		if(!isActive)return;
		GUIManager.Instance.OnPressTapLayer(isPress);

		if(isPress){
			SetUIPositon(GetTouchPosition(),tapIndecator);
			if(!tapIndecator.gameObject.activeSelf)tapIndecator.gameObject.SetActive(true);
		}else{
			if(tapIndecator.gameObject.activeSelf)tapIndecator.gameObject.SetActive(false);
		}

	}
	void OnClick(){
		if(!isActive)return;
		SetUIPositon(GetTouchPosition(),tapIndecator);
		GUIManager.Instance.OnClickTapLayer(InverseOverlayPosition(GetTouchPosition()));
	}

	void OnDrag (Vector2 delta){
		if(!isActive)return;
		SetUIPositon(GetTouchPosition(),tapIndecator);
		GUIManager.Instance.OnClickTapLayer(InverseOverlayPosition(GetTouchPosition()));
	}

	Vector3 GetTouchPosition(){
		return UICamera.lastWorldPosition;
	}

	[SerializeField]
	private LayerMask layerMask;
	Vector3 worldPos;
	Ray ray ;
	RaycastHit hit;
	Vector3 InverseOverlayPosition (Vector3 tapPos)
	{
		
		worldPos=uiCam.WorldToScreenPoint(tapPos);

		ray = mainCam.ScreenPointToRay(worldPos);

		hit = new RaycastHit();
//		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
		if (Physics.Raycast(ray, out hit, 10,layerMask)) {
			return new Vector3(hit.point.x,hit.collider.gameObject.transform.position.y,hit.point.z);
		}else{
			Debug.LogError("Tapdetecter is not found");
			return Vector3.zero;

		}



	}
}
