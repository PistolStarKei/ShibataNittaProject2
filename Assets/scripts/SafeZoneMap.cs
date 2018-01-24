using UnityEngine;
using System.Collections;

public class SafeZoneMap : MonoBehaviour {
	
	public Transform playerOBJ;
	public float width;
	float temp=0.0f;
	void MovePlayer(){
		temp=width/Mathf.Abs(PSPhoton.GameManager.instance.safeZone.topLeft.position.x);
		playerOBJ.localPosition=new Vector3(GUIManager.Instance.shipControll.transform.position.x*temp
			,GUIManager.Instance.shipControll.transform.position.z*temp,0.0f);
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
	}

	public float tweenFlom=0.1f;
	public float tweenTo=0.7f;
	public float tweenDulation=0.7f;
	public Color colDefault;
	public Color colDanger;

}
