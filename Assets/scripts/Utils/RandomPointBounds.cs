using UnityEngine;
using System.Collections;

namespace PS_Util{
	
	public abstract class RandomPointBounds : MonoBehaviour {

		void Awake(){
			GetBounds();
		}

		public abstract void GetBounds();
			
		public virtual void DestroyComponets(){
			Component[] components = gameObject.GetComponents(typeof(Component));

			for(int i=0; i<components.Length;i++){
				if( components[i] != this && components[i].GetType()!=typeof(Transform)){

					Destroy(components[i]);

				}
			}
		}
		public virtual void MatchScale(){
		}
		public virtual void GetColliderBounds(){
		}
		public virtual Vector3 GetRandomPointInArea(){
			return Vector3.zero;
		}
	}
}