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

		//候補の格納
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

		//すでに全てがデンジャーゾーン
		if(vect.Count<=0)return null;
		//残りのひとマス
		if(vect.Count==1)return vect[0];


		IntVector2 kouho=new IntVector2();
		kouho.x=-100;
		while(vect.Count>0){

			kouho=vect[Random.Range(0,vect.Count)];
			//評価する
			if(IsReachable(kouho)){
				break;
			}else{
				vect.Remove(kouho);
			}
		}

		if(kouho.x==-100){
			return null;
		}

		return kouho;

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

	[SerializeField]
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





	#region 次の候補の検索
	bool[] safezoneTemp;
	void SetSafeZoneTemp(int x,int y,bool isSafeZone){
		safezoneTemp[x*PSParams.GameParameters.mapMasuXY+y]=isSafeZone;
	}
	bool isSafeZoneTemp(int x,int y){
		return safezoneTemp[x*PSParams.GameParameters.mapMasuXY+y];
	}

	//いかがメソッド ランダムの基礎になる
	public bool IsReachable(IntVector2 kouho){
		if(!isSafeZone(kouho.x,kouho.y)){
			//すでに危険地帯なので候補外
			return false;
		}

		//仮に写す。
		safezoneTemp=new bool[safezone.Length];
		int safeZoneNum=0;
		IntVector2 startPoint=new IntVector2();
		startPoint.x=-100;

		for(int r=0;r<PSParams.GameParameters.mapMasuXY;r++){
			for(int v=0;v<PSParams.GameParameters.mapMasuXY;v++){

				if(isSafeZone(r,v))safeZoneNum++;

				if(kouho.x==r && kouho.y==v){
					//候補を危険地帯に設定
					SetSafeZoneTemp(kouho.x,kouho.y,false);
				}else{
					if(isSafeZone(r,v)){
						if(startPoint.x==-100){
							startPoint=new IntVector2();
							startPoint.x=r;
							startPoint.y=v;
						}
					}

					SetSafeZoneTemp(r,v,isSafeZone(r,v));
				}
			}
		}
		if(safeZoneNum<=0){
			//すでに全部危険地帯
			return false;
		}


		//startPoint;で探索開始
		reachedPoints.Clear();

		FloodFill(startPoint.x,startPoint.y);

		//safeZoneNum-1と到達リストカウントが同じならば良い。
		if(reachedPoints.Count==safeZoneNum-1){
			return true;
		}


		return false;
	}


	//到達した領域をここに保存する
	List<int> reachedPoints=new List<int>();
	void AddReached(int x,int y){
		reachedPoints.Add(x*PSParams.GameParameters.mapMasuXY+y);
	}

	IntVector2 intVec;
	void FloodFill(int x, int y)
	{

		if(x<0 || x>=PSParams.GameParameters.mapMasuXY)return;
		if(y<0 || y>=PSParams.GameParameters.mapMasuXY)return;

		if(isSafeZoneTemp(x,y) && !reachedPoints.Contains(x*PSParams.GameParameters.mapMasuXY+y)){
			AddReached(x,y);
			FloodFill(x+1,y);
			FloodFill(x,y+1);
			FloodFill(x-1,y);
			FloodFill(x,y-1);
		}


	}
	#endregion



}
