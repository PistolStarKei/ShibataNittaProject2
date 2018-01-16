using UnityEngine;
using System.Collections;

namespace PS_Util{
	public class RandomPointBoundsBox : RandomPointBounds {


		public override void GetBounds(){
			if(transform.localRotation.eulerAngles!=Vector3.zero){
				Debug.LogError(" RandomPointBoundsBox Transformのrotationはゼロでないといけなません");
			}

			col=gameObject.GetComponent<BoxCollider>();
			GetColliderBounds();
		}


		BoxCollider col;
		public override void MatchScale(){
			col.size=new Vector3(col.size.x*transform.lossyScale.x,col.size.y*transform.lossyScale.y,col.size.z*transform.lossyScale.z);
			transform.localScale=Vector3.one;
		}

		Vector3 center;
		Vector3 size;
		public override void GetColliderBounds(){
			if(col){
				MatchScale();

				center=transform.InverseTransformPoint(col.bounds.center);
				size=col.size/2.0f;

				DestroyComponets();
			}
		}
		public override Vector3 GetRandomPointInArea(){
			Vector3 rndPosWithin=transform.TransformPoint(center+new Vector3(Random.Range(-size.x, size.x), 
				Random.Range(-size.y, size.y), 
				Random.Range(-size.z, size.z)));;
			return rndPosWithin;
		}
	}
}
