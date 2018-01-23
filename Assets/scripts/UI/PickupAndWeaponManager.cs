using UnityEngine;
using System.Collections;
using PathologicalGames;

public enum DanmakuColor{White,Blue,Yellow,Green,Red};
public class PickupAndWeaponManager : PS_SingletonBehaviour<PickupAndWeaponManager> {


	public string poolName="Weapons";

	public GameObject[] pu_prefabs;

	public GameObject[] danmaku;

	public GameObject subweaponYudoudan_Prefab;
	public Transform SpawnSubweapon_Yudoudan(shipControl ship,Vector3 position,Quaternion qt,float spawnTime,ShipOffset offset,string ID,int shipID){
		Transform trans=Spawn(subweaponYudoudan_Prefab,position,qt,null);
		trans.gameObject.GetComponent<Subweapon_Yudoudan>().Spawn(ship,spawnTime,position,offset,ID,shipID);
		return trans;

	}


	public GameObject subweaponNukeEffecter_Prefab;
	public Transform SpawnSubweapon_NukeEffecter(shipControl ship,Vector3 position,Quaternion qt,Transform parent){
		Transform trans=Spawn(subweaponNukeEffecter_Prefab,position,qt,parent);
		trans.gameObject.GetComponent<SubweaponEffecter>().Spawn(ship);
		return trans;

	}


	public GameObject subweaponNuke_Prefab;
	public Transform SpawnSubweapon_Nuke(shipControl ship,Vector3 position,Quaternion qt,float spawnTime,ShipOffset offset,string ID){
		Transform trans=Spawn(subweaponNuke_Prefab,position,qt,null);
		trans.gameObject.GetComponent<SubweaponShot>().Spawn(ship,spawnTime,position,offset,ID);
		return trans;

	}


	public GameObject subweaponNapamEffecter_Prefab;
	public Transform SpawnSubweapon_NapamEffecter(shipControl ship,Vector3 position,Quaternion qt,Transform parent){
		Transform trans=Spawn(subweaponNapamEffecter_Prefab,position,qt,parent);
		trans.gameObject.GetComponent<SubweaponEffecter>().Spawn(ship);
		return trans;

	}


	public GameObject subweaponNapam_Prefab;
	public Transform SpawnSubweapon_Napam(shipControl ship,Vector3 position,Quaternion qt,float spawnTime,ShipOffset offset,string ID){
		Transform trans=Spawn(subweaponNapam_Prefab,position,qt,null);
		trans.gameObject.GetComponent<SubweaponShot>().Spawn(ship,spawnTime,position,offset,ID);
		return trans;

	}

	public GameObject subweaponZenhoukou_Prefab;
	public Transform SpawnSubweapon_Zenhoukou(shipControl ship,Vector3 position,Quaternion qt,float spawnTime,ShipOffset offset,string ID){
		Transform trans=Spawn(subweaponZenhoukou_Prefab,position,qt,null);
		trans.gameObject.GetComponent<SubweaponShot>().Spawn(ship,spawnTime,position,offset,ID);
		return trans;

	}

	public GameObject subweaponWave_Prefab;
	public Transform SpawnSubweapon_Wave(shipControl ship,Vector3 position,Quaternion qt,float spawnTime,ShipOffset offset,string ID){
		Transform trans=Spawn(subweaponWave_Prefab,position,qt,null);
		if(ship)trans.gameObject.GetComponent<SubweaponShot>().Spawn(ship,spawnTime,position,offset,ID);
		return trans;

	}

	public Transform SpawnPickUpItem(Pickup putype,Vector3 position,Quaternion qt,Transform parent){
		return Spawn(pu_prefabs[(int)putype],position,qt,parent);
	}

	public Transform SpawnShot(shipControl ship,DanmakuColor col,Vector3 position,Quaternion qt,float spawnTime,ShipOffset offset,string ID){

		Transform trans=Spawn(danmaku[(int)col],position,qt,null);
		if(ship)trans.gameObject.GetComponent<shot>().Spawn(ship,spawnTime,position,offset,ID);
		return trans;
	}


	Transform current;
	Transform Spawn(GameObject prefab,Vector3 position,Quaternion qt,Transform parent){


		if(parent!=null){
			current=PoolManager.Pools[poolName].Spawn
				(
					prefab, 
					position, 
					qt,
					parent
				);
		}else{
			current=PoolManager.Pools[poolName].Spawn
				(
					prefab, 
					position, 
					qt
				);
		}

		return current;
	}
}
