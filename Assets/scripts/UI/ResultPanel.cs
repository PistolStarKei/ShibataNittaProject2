using UnityEngine;
using System.Collections;

public class ResultPanel : MonoBehaviour {


	public PS_GUI_Cover cover;

	public void ShowResult(int rank,int kills){
		isShowing=true;
		rankLb.text=rank.ToString();
		killLb.text=kills.ToString();
		ta_panel.PlayForward();
		cover.CoverWithBlackMask();
	}

	public void CloseResult(){
		ta_panel.PlayReverse();
	}

	public void OnCliclMain(){
		
	}

	public UILabel rankLb;
	public UILabel killLb;

	public bool isShowing=false;

	public TweenAlpha ta_panel;
	public void OnTweenPanel(){
		if(ta_panel.direction==AnimationOrTween.Direction.Forward){
			tp_panel.PlayForward();
		}else if(ta_panel.direction==AnimationOrTween.Direction.Reverse){
			isShowing=false;
		}
	}


	public TweenPosition tp_panel;
	public void OnTweenButton(){
		if(tp_panel.direction==AnimationOrTween.Direction.Forward){

		}else if(tp_panel.direction==AnimationOrTween.Direction.Reverse){
			
		}
	}
}
