using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]

public class EngineParticle : MonoBehaviour {

	void Start(){

		for(int i=0;i<ps.Length;i++){
			ps[i].loop=false;
			ps[i].Stop();
		}

	}

	public void Engine(bool isOn){
		if(isOn){
			for(int i=0;i<ps.Length;i++){
				ps[i].loop=true;
				ps[i].Play();
			}

		}else{
			if(!ps[0].isStopped){
				for(int i=0;i<ps.Length;i++){
					ps[i].loop=false;
					ps[i].Stop();
				}
			}
		}
	}
	public ParticleSystem[] ps;
}
