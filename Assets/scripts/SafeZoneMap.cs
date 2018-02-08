using UnityEngine;
using System.Collections;

public class SafeZoneMap : MonoBehaviour {
	
	public Transform playerOBJ;
	public Transform mapContainer;
	public UITexture mapTex;

	public float width;
	public float bairitsu=1f;

	public UILabel plus;
	public UILabel minus;
	public int current=0;
	float[] bairitsus=new float[3]{1.0f,2.0f,4.0f};
	public void OnClickPlus(){
		current++;
		if(current>=bairitsus.Length){
			current=bairitsus.Length-1;
			return;
		}
		if(current==bairitsus.Length-1){
			plus.color=Color.black;
		}
		minus.color=Color.white;
		SetBairitsu(bairitsus[current]);
	}
	public void OnClickMinus(){
		current--;
		if(current<0){
			current=0;
			return;
		}
		if(current==0){
			minus.color=Color.black;
		}
		plus.color=Color.white;

		SetBairitsu(bairitsus[current]);
	}

	public void SetBairitsu(float bairitsu){
		this.bairitsu=bairitsu;
		mapTex.width=(int)(bairitsu*333);
		mapTex.height=(int)(bairitsu*333);

		grid.transform.localScale=new Vector3(bairitsu,bairitsu,bairitsu);
	}
	public void SetMapTexture(Texture2D tex){
		mapTex.mainTexture=tex;
	}

	void ApplyGridSize(float bairitsu){

		foreach(SafeZoneMapItem smi in sps){
			smi.SetSize(Mathf.FloorToInt(50*bairitsu));
		}
		grid.cellWidth=50*bairitsu;
		grid.cellHeight=50*bairitsu;
		grid.Reposition();
	}

	float temp=0.0f;
	void MovePlayer(){
		temp=(width*bairitsu)/Mathf.Abs(PSPhoton.GameManager.instance.safeZone.topLeft.position.x);

		/*playerOBJ.localPosition=new Vector3(GUIManager.Instance.shipControll.transform.position.x*temp
			,GUIManager.Instance.shipControll.transform.position.z*temp,0.0f);*/

		mapContainer.localPosition=new Vector3(-(GUIManager.Instance.shipControll.transform.position.x*temp)
			,-(GUIManager.Instance.shipControll.transform.position.z*temp),0.0f);
		
	}


	void LateUpdate(){

		if(playerOBJ && GUIManager.Instance.shipControll){
			MovePlayer();
			//自機の表示
			if(GUIManager.Instance.shipControll){
				playerOBJ.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,-GUIManager.Instance.shipControll.transform.eulerAngles.y));
			}
		
		}
	}

	public SafeZoneMapItem[] sps;
	public UIGrid grid;
	public void SetState(int x,int y,int masuXY,ZoneState state){
		sps[x*masuXY+y].SetStateTo(state);
	}

	void Awake(){
		sps=new SafeZoneMapItem[ grid.GetChildList().Count];
		int i=0;
		foreach(Transform tr in grid.GetChildList()){
			sps[i]=tr.gameObject.GetComponent<SafeZoneMapItem>();
			sps[i].mapParent=this;
			i++;
		}
		current=2;
		SetBairitsu(bairitsus[current]);
	}

	public float tweenFlom=0.0f;
	public float tweenTo=0.2f;
	public float tweenDulation=0.7f;
	public Color colDefault;
	public Color colDanger;


}
