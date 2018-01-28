using UnityEngine;
using System.Collections;

public enum TapViewType{Side,Top,Forward}
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
		isPressed=isPress;
		if(!isActive)return;

		if(GUIManager.Instance!=null){
			GUIManager.Instance.OnPressTapLayer(isPress,InverseOverlayPosition(GetTouchPosition()));
		}else{
			if(ShipSwitcher.Instance!=null && ShipSwitcher.Instance.currentShip)ShipSwitcher.Instance.currentShip.OnPress(isPress,InverseOverlayPosition(GetTouchPosition()));
		}

		if(isPress){
			SetUIPositon(GetTouchPosition(),tapIndecator);
			if(!tapIndecator.gameObject.activeSelf)tapIndecator.gameObject.SetActive(true);
		}else{
			if(tapIndecator.gameObject.activeSelf)tapIndecator.gameObject.SetActive(false);
		}

	}

	bool isPressed=false;

	void Update(){

		if(isPressed && isActive){
			if(GUIManager.Instance!=null){
				GUIManager.Instance.OnUpdateTapLayer(InverseOverlayPosition(GetTouchPosition()));
			}else{
				if(ShipSwitcher.Instance!=null && ShipSwitcher.Instance.currentShip)ShipSwitcher.Instance.currentShip.OnTapUpdate(InverseOverlayPosition(GetTouchPosition()));
			}
		}


	}
			/*
	void OnClick(){
		if(!isActive)return;
		SetUIPositon(GetTouchPosition(),tapIndecator);
		GUIManager.Instance.OnClickTapLayer(InverseOverlayPosition(GetTouchPosition()));
	}*/

	void OnDrag (Vector2 delta){
		if(!isActive)return;
		SetUIPositon(GetTouchPosition(),tapIndecator);
	}

	Vector3 GetTouchPosition(){
		return UICamera.lastWorldPosition;
	}


	public TapViewType viewType=TapViewType.Top;
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
		if (Physics.Raycast(ray, out hit, 1000,layerMask)) {
			switch(viewType){
				case TapViewType.Top:
					return new Vector3(hit.point.x,hit.collider.gameObject.transform.position.y,hit.point.z);
					break;
				case TapViewType.Forward:
					return new Vector3(hit.point.x,hit.point.y,hit.collider.gameObject.transform.position.y);
					break;
				case TapViewType.Side:
					return new Vector3(hit.collider.gameObject.transform.position.x,hit.point.y,hit.point.z);
					break;
			}

		}else{
			Debug.LogError("Tapdetecter is not found");


		}
		return Vector3.zero;


	}
}
