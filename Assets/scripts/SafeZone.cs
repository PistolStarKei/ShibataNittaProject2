using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class IntVector2
{
	public int x;
	public int y;

}
public class SafeZone : MonoBehaviour {
	

	public Transform topLeft;
	public Transform btnRight;

	float extent=0.0f;

	void Start(){
		InitSafeZone(PSParams.GameParameters.mapMasuXY,topLeft.position,btnRight.position);
	}

	public IntVector2 GetNextDangerZoneRandom(){
		List<IntVector2> vect=new List<IntVector2>();


		for(int r=0;r<PSParams.GameParameters.mapMasuXY;r++){
			for(int v=0;v<PSParams.GameParameters.mapMasuXY;v++){
				if(isSafeZone(r,v)){
					IntVector2 temp=new IntVector2();
					temp.x=r;
					temp.y=v;
					vect.Add(temp);
				}
			}
		}

		if(vect.Count<=0)return null;

		return vect[Random.Range(0,vect.Count)];

	}

	public void InitSafeZone(int masuXY,Vector3 tl,Vector3 br){
		safezone=new bool[masuXY*masuXY];
		for(int i=0;i<masuXY*masuXY;i++){
			safezone[i]=true;
		}

		extent=(Mathf.Abs(br.x)+Mathf.Abs(tl.x))/(masuXY*2);

		centers=new Vector3[safezone.Length];

		float[] xPOS=new float[masuXY];
		xPOS[0]=tl.x+extent;
		for(int e=1;e<masuXY;e++){
			xPOS[e]=xPOS[e-1]+(extent*2.0f);
		}

		float[] yPOS=new float[masuXY];
		yPOS[0]=tl.z-extent;
		for(int f=1;f<masuXY;f++){
			yPOS[f]=yPOS[f-1]-(extent*2.0f);
		}


		for(int r=0;r<masuXY;r++){
			for(int v=0;v<masuXY;v++){
				SetCenters(r,v,new Vector3(xPOS[v],0.0f,yPOS[r]));
			}
		}

	}

	public bool IsInSafeZone(Vector3 position){
		bool isSafe=true;
		for(int r=0;r<PSParams.GameParameters.mapMasuXY;r++){
			for(int v=0;v<PSParams.GameParameters.mapMasuXY;v++){
				if(!isSafeZone(r,v)){
					if(isInZone(position,GetCenterPos(r,v))){
						isSafe=false;
					}
				}

			}
		}

		return isSafe;
	}





	//[SerializeField]
	Vector3[] centers;
	void SetCenters(int x,int y,Vector3 centerPos){
		centers[x*PSParams.GameParameters.mapMasuXY+y]=centerPos;
	}
	Vector3 GetCenterPos(int x,int y){
		return centers[x*PSParams.GameParameters.mapMasuXY+y];
	}

	//[SerializeField]
	bool[] safezone;
	public bool isSafeZone(int x,int y){
		return safezone[x*PSParams.GameParameters.mapMasuXY+y];
	}
	public void SetSafeZone(int x,int y,bool isSafe){
		safezone[x*PSParams.GameParameters.mapMasuXY+y]=isSafe;
	}	

	bool isInZone(Vector3 position,Vector3 center){
		Bounds boud=new Bounds();
		boud.center=center;
		boud.extents=new Vector3(extent,extent,extent);
		return boud.Contains(position);
	}









}
