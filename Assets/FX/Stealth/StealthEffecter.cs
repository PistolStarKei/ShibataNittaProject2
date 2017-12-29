using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (MeshRenderer))]
public class StealthEffecter : MonoBehaviour {

	bool isStealthMode=false;
	public bool needStealthMode=false;
	public void StealthMode(bool isOn)
	{
		if(!isStealthMode){
			if(isOn){
				StealthOn();
			}else{
				Debug.LogError("すでにステルスモードではない");
			}
		}else{
			if(isOn){
				Debug.LogError("すでにステルスモードです");
			}else{
				StealthOff();
			}
		}
	}

	public Material glassMat;

	Material[] matList; 
	void StealthOn(){
		matList=new Material[mr.materials.Length];

		Material[] matListGlass=new Material[matList.Length]; 
		for(int i=0; i<matList.Length;i++ ){
			matList[i]=mr.materials[i];
			matListGlass[i]=glassMat;
		}
		mr.materials=matListGlass;
		isStealthMode=true;


	}

	void StealthOff(){
		mr.materials=matList;
		matList=null;
		isStealthMode=false;
	}

	MeshRenderer mr;
	void Start(){
		mr=gameObject.GetComponent<MeshRenderer>() as MeshRenderer;

	}

	Color lerpedColor;
	void Update() {
		if(isStealthMode && needStealthMode){
			lerpedColor = Color.Lerp(colorOnStealthMode,colorOnStealthMode_to, Mathf.PingPong(Time.time, 1));
			for(int i=0;i<mr.materials.Length;i++){
				mr.materials[i].SetColor("_ReflectColor",lerpedColor);
			}
		}
	}

	public Color colorOnStealthMode=new Color(0.0f,255.0f/255.0f,212.0f/255.0f,127.0f/255.0f);
	public Color colorOnStealthMode_to=new Color(00.0f,0.0f,0.0f,127.0f/255.0f);


}
