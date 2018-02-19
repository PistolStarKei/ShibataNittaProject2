using UnityEngine;
using System.Collections;

public class SubweaponEffecter_Nuke : SubweaponEffecter {


	public override void Spawn(shipControl launcherShip){
		this.launcherShip=launcherShip;
		if(GUIManager.Instance.IsWithinAudioDistance(transform.position))AudioController.Play("Nuke",transform.position,null);

	}


	public override void Effect(shipControl ship){
		
		ParticleManager.Instance.ShowExplosionSmallAt(ship.transform.position,Quaternion.identity,null);
		GUIManager.Instance.ShakeCameraBig();
	}
}
