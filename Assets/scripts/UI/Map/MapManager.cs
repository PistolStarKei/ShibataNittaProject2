using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {


	public List<UISprite> enemies=new List<UISprite>();

	[Tooltip("マップ画面のローカルポジション上の半径")]
	public float radius=20.0f;

	void UpdateEnemyPosition(UISprite sp,Vector2 vec){
		sp.transform.localPosition=new Vector3(vec.x*radius,vec.y*radius,0.0f);
	}
		

	void LateUpdate(){
		
		if(mapDetectTrigger && mapDetectTrigger.playerTrans){
			
			//足りない分を作り、余った分は消す
			if(enemies.Count!=mapDetectTrigger.ships.Count){
				int sabun=mapDetectTrigger.ships.Count-enemies.Count;
				//2-0

				if(sabun>0){
					for(int i=0;i<sabun;i++){
						AddEnemies();
					}
				}else{
					for(int i=0;i<Mathf.Abs(sabun);i++){
						RemoveEnemies();
					}
				}
			}
			if(enemies.Count>0){
				int num=0;
				foreach(shipControl ship in mapDetectTrigger.ships){
					UpdateEnemyPosition(enemies[num],mapDetectTrigger.GetRelativePosition(ship.transform.position));
					num++;
				}
			}
		}

		if(GUIManager.Instance.shipControll){
			transform.localRotation=Quaternion.Euler(new Vector3(0.0f,0.0f,-GUIManager.Instance.shipControll.transform.eulerAngles.y));
		}
	}

	public UISprite  playerSp;

	public MapDetecterTrigger mapDetectTrigger;

	public Transform mapContent;

	public GameObject enemySP;

	void AddEnemies(){
		GameObject o=Instantiate(enemySP,Vector3.zero,Quaternion.identity) as GameObject;
		o.transform.parent=mapContent;
		o.transform.localPosition=Vector3.zero;
		o.transform.localRotation=Quaternion.identity;
		o.transform.localScale=Vector3.one;
		UISprite sp=o.GetComponent<UISprite>();
		enemies.Add(sp);
	}

	void RemoveEnemies(){
		Destroy(enemies[enemies.Count-1].gameObject);
		enemies.RemoveAt(enemies.Count-1);
	}


}
