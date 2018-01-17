using UnityEngine;
using System.Collections;

public class Subweapon_Wave : SubweaponShot {

	public float shotSpeed=1.0f;
	public override void Move(){
		transform.position=spawnPos+(transform.forward*(ellapsedTime*shotSpeed));
	}

	public  override void OnCollideShip(shipControl ship,Vector3 hitpoint){
		base.OnCollideShip(ship,hitpoint);
	}

	public override  void OnCollideWall(){
		base.OnCollideWall();

	}


}
