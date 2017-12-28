using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (UIGrid))]

public class PS_GUI_DynamicInfo : MonoBehaviour {
	[SerializeField]
	List<PS_GUI_DynamicInfoObject> infoLists=new List<PS_GUI_DynamicInfoObject>();
	UIGrid grid;

	public List<GameObject> infoObjPool=new List<GameObject>();
	GameObject GetFromPool(){
		foreach(GameObject go in infoObjPool){
			if(!go.GetComponent<PS_GUI_DynamicInfoObject>().isEnabled){
				return go;
				break;
			}
		}
		return null;
	}

	// Use this for initialization
	void Start () {
		grid=gameObject.GetComponent<UIGrid>();
	
	}
	public float infoDulation=4.0f;
	public Transform spawnPos;
	public void Log(string str){
		if(infoLists.Count>=2){
			infoLists[0].DestroyObjNow();
		}
		GameObject b = GetFromPool();

		PS_GUI_DynamicInfoObject obj=b.GetComponent<PS_GUI_DynamicInfoObject>() as PS_GUI_DynamicInfoObject;
		if(obj==null)Debug.LogError("obj ==null");
		b.transform.parent = transform;
		b.transform.position=spawnPos.position;
		b.transform.localScale = Vector3.one;
		infoLists.Add(obj);
		obj.Init(str,this,infoDulation);
		NGUITools.SetActive ( b, true );
		grid.Reposition();
	}

	public void RemoveFromList(PS_GUI_DynamicInfoObject obj){
		infoLists.Remove(obj);
	}

}
