using UnityEngine;
using System.Collections;

public class SubweaponEffecter_Napam : SubweaponEffecter {

	public override void Spawn(shipControl launcherShip){
		this.launcherShip=launcherShip;
		if(GUIManager.Instance.IsWithinAudioDistance(transform.position))AudioController.Play("Napam",transform.position,null);

	}

	public override void Effect(shipControl ship){
		ParticleManager.Instance.ShowExplosionSmallAt(ship.transform.position,Quaternion.identity,ship.transform);
		GUIManager.Instance.ShakeCameraBig();
	}

}
