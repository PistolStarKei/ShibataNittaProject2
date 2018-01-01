using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]

public class EngineParticle : MonoBehaviour {

	void Start(){
		ps=gameObject.GetComponent<ParticleSystem>();
		ps.loop=false;
		ps.Stop();
	}

	public float speed=1.0f;
	public void Engine(bool isOn){
		if(isOn){
			ps.loop=true;
			ps.Play();
		}else{
			if(!ps.isStopped){
				ps.loop=false;
				ps.Stop();
			}
		}
	}
	ParticleSystem ps;
}
