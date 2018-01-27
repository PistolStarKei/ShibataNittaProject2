using UnityEngine;
using System.Collections;

namespace PSGUI{
	
	public class FollowGUI : MonoBehaviour {

		internal UIWidget widget;
		// Use this for initialization
		void Awake () {
			widget=GetComponent<UIWidget>();
		}

		public Transform target;
		public virtual void SetTarget(Transform target){
			this.target=target;

		}
		// Update is called once per frame

		public Vector3 offset;

		internal Vector3 vec;
		void LateUpdate () {
			if(widget!=null){
				OverlayPosition();
			}
		}


		public virtual void OverlayPosition(){
			if(isInsideView()){
				vec=GetOverlayPosition(GetViewPortPointOfTarget());
				vec.z=0.0f;
				vec+=offset;
				widget.transform.localPosition=vec;
			}else{
				vec=GetOverlayPosition(GetViewPortPointOfTarget());
				vec.z=0.0f;
				vec+=offset;
				widget.transform.localPosition=vec;
			}
		}


		internal bool isInsideView(){
			vec=GetViewPortPointOfTarget();
			if(vec.x<0.0f)return false;
			if(vec.y<0.0f)return false;
			if(vec.y>1.0f)return false;
			if(vec.x>1.0f)return false;
			return true;
		}
			
		internal Vector3 GetViewPortPointOfTarget(){
			Camera worldCam = NGUITools.FindCameraForLayer(target.gameObject.layer);
			if (worldCam){
				return worldCam.WorldToViewportPoint(target.gameObject.transform.position);
			}
			return Vector3.zero;
		}

		internal Vector3 GetOverlayPosition (Vector3 targetViewPointPoint)
		{
			Camera myCam = NGUITools.FindCameraForLayer(transform.gameObject.layer);
			if (myCam != null) {
				targetViewPointPoint = myCam.ViewportToWorldPoint(targetViewPointPoint);
				return (transform.parent != null) ? transform.parent.InverseTransformPoint(targetViewPointPoint) : targetViewPointPoint;
			}
			return Vector3.zero;
		}

		internal void Show(bool isShow){
				widget.alpha=!isShow?0.0f:1.0f;

		}
	}
}
