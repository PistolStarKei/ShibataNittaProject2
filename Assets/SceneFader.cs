using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PSGUI{
	public class SceneFader : MonoBehaviour {


		public TweenAlpha ta;
		public BoxCollider coverCollider;

		public delegate void Callback_OnTweened();
		public event Callback_OnTweened onTweened;

		bool needCoverImmediate=false;
		public void FadeOut(Callback_OnTweened onTweened,bool needCoverImmediate){
			this.needCoverImmediate=needCoverImmediate;
			if(needCoverImmediate)coverCollider.enabled=true;
			this.onTweened=onTweened;
			NGUITools.SetActive(gameObject,true);
			ta.PlayForward();
		}

		public void FadeIn(Callback_OnTweened onTweened,bool needUnCoverImmediate){
			this.needCoverImmediate=needUnCoverImmediate;
			if(needCoverImmediate)coverCollider.enabled=false;
			this.onTweened=onTweened;
			ta.PlayReverse();
		}

		public void OnTweened(){
			if(ta.direction==AnimationOrTween.Direction.Forward){
				if(!needCoverImmediate)coverCollider.enabled=true;
				if(onTweened!=null)onTweened();
			}else if(ta.direction==AnimationOrTween.Direction.Reverse){
				if(!needCoverImmediate)coverCollider.enabled=false;
				if(onTweened!=null)onTweened();
				NGUITools.SetActive(gameObject,false);
			}
		}
	}
}
