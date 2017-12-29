using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (MeshRenderer))]
public class StealthEffecter : MonoBehaviour {


	public bool isStealthMode=false;
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
}
