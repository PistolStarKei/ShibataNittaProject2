using UnityEngine;
using System.Collections;

[RequireComponent (typeof (UILabel))]
public class PS_GUI_DynamicInfoObject : MonoBehaviour {

	public bool isEnabled=false;
	public UILabel label;
	public int offset=5;
	TweenAlpha ta;
	TweenPosition tp;
	PS_GUI_DynamicInfo info;
	void Start(){
		ta=GetComponent<TweenAlpha>();
		tp=GetComponent<TweenPosition>();
		GetComponent<UIWidget>().alpha=1.0f;
	}
	public void Init(string str,PS_GUI_DynamicInfo info,float dulation){
		isEnabled=true;
		GetComponent<UIWidget>().alpha=1.0f;
		this.info=info;
		label.text=str;
		gameObject.SetActive(true);
		Invoke("DestroyObj",dulation);
	}

	public void DestroyObj(){
		CancelInvoke("DestroyObj");
		ta.ResetToBeginning();
		tp.ResetToBeginning();
		ta.PlayForward();
		tp.from=transform.localPosition;
		tp.to=new Vector3(tp.from.x,tp.from.y+offset,tp.from.z);
		tp.PlayForward();
	}

	public void DestroyObjNow(){
		CancelInvoke("DestroyObj");
		info.RemoveFromList(this);
		isEnabled=false;
		gameObject.SetActive(false);
		transform.parent=info.spawnPos;
	}

	public void OnTweened(){
		if(ta.direction==AnimationOrTween.Direction.Forward){
			info.RemoveFromList(this);
			isEnabled=false;
			gameObject.SetActive(false);
			transform.parent=info.spawnPos;
		}else if(ta.direction==AnimationOrTween.Direction.Reverse){
			
		}
	}
}
