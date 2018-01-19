using UnityEngine;
using System.Collections;

public class Subweapon_Wave : SubweaponShot {

	public float shotSpeed=1.0f;
	public override void Move(){
		//transform.Translate(Vector3.forward * Time.deltaTime*shotSpeed);
		transform.position= GetEllapsedPosition(spawnPos,transform.forward,ellapsedTime);
	}

	Vector3 GetEllapsedPosition(Vector3 spawnAt,Vector3 vector,float ellapsedTime){
		return spawnAt+vector*(ellapsedTime*shotSpeed);
	}

	public  override void OnCollideShip(shipControl ship,Vector3 hitpoint){
		base.OnCollideShip(ship,hitpoint);
	}

	public override  void OnCollideWall(){
		base.OnCollideWall();

	}


}
