using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TicketParticle : MonoBehaviour {

	#region  Public変数
	[Header("参照用")]
	public UISprite mSprite;
	public ParticleSystem mParticle;
	
	[Header("Public変数")]
	[Space(10)]
	public float dulation=5f;
	public Vector3 particlePosition=new Vector3(5.56f,-8.41f,0f);
	#endregion

	#region  メンバ変数
	float time=0f;
	#endregion

	#region  初期化
	void Awake () {
		this.mSprite=gameObject.GetComponent<UISprite>();
		this.mParticle=GetComponentInChildren<ParticleSystem>();
		if(this.mParticle!=null)this.mParticle.gameObject.transform.localPosition=particlePosition;
	}
	#endregion

	#region  Update
	void Update () {
		if(mSprite.enabled){
			time+=Time.deltaTime;
			if(time>dulation){
				time=0f;
				mParticle.Play();
			}
		}else{
			time=0f;
			if(mParticle.isPlaying)mParticle.Stop();
		}
	}
	#endregion

}
