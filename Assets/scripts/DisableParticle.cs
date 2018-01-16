using UnityEngine;
using System.Collections;
using PathologicalGames;

public class DisableParticle : MonoBehaviour 
{
	void OnEnable()
	{
		StartCoroutine (ParticleWorking ());
	}


	IEnumerator ParticleWorking()
	{
		var particle = GetComponent<ParticleSystem> ();

		yield return new WaitWhile (() => particle.IsAlive (true));

		PoolManager.Pools["Particles"].Despawn(gameObject.transform);
	}
}
