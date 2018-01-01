using UnityEngine;
using System.Collections;

public class ShipSelecter : MonoBehaviour {

	public ScrollItem[] items;
	public bool[] itemFlags;


	void Start(){
		ClearAll();
		UpdateItems();
	}

	void ClearAll(){
		for(int i=0;i<itemFlags.Length;i++){
			itemFlags[i]=false;

		}
	}
	void UpdateItems(){
		for(int i=0;i<itemFlags.Length;i++){
			items[i].SetState(itemFlags[i]);
		}
	}

	public int currentSelect=0;
	public void OnClickItem(string name){
		ClearAll();
		currentSelect=int.Parse(name)-1;
		itemFlags[currentSelect]=true;
		UpdateItems();
	}

	public float enableAlpha=0.1f;
	public float disbleAlpha=1.0f;

}
