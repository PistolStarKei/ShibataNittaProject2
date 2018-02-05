using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Test :MonoBehaviour {
	
	public ParticleSystem psF;
	void EmitForwad(int scale){
		psF.Emit(scale);
	}

	public int emit=1;
	void Start () {
		StartCoroutine( Emitter ());
	}
	IEnumerator Emitter () {
		while(true){
			EmitForwad(emit);
			yield return new WaitForSeconds(.5f);
		}
	}
}
