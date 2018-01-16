using UnityEngine;
using System.Collections;

public class Subweapon_Nuke : SubweaponShot {

	public float shotSpeed=1.0f;
	public override void Move(){
		transform.Translate(Vector3.forward * Time.deltaTime*shotSpeed);

	}

	public override void KillTimer(){
		//爆破オブジェクトをスポーンして居なくなる
		PickupAndWeaponManager.Instance.SpawnSubweapon_NukeEffecter(launcherShip,this.transform.position,Quaternion.identity,null);
		KillSelf();
	}

	public  override void OnCollideShip(shipControl ship,Vector3 hitpoint){
		if(ship){
			if(ship!=launcherShip){
				//発射した機体以外の場合
				KillTimer();


			}else{
				//自機であった場合
			}
		}
	}

	public override  void OnCollideWall(){
		KillTimer();

	}
}
