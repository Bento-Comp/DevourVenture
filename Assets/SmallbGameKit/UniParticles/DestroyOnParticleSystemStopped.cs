using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniParticles
{
	[AddComponentMenu("Template/DestroyOnParticleSystemStopped")]
	public class DestroyOnParticleSystemStopped : MonoBehaviour
	{
		public GameObject gameObjectToDestroy;

		void Start()
		{
			ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
			main.stopAction = ParticleSystemStopAction.Callback;
		}

		void OnParticleSystemStopped()
		{
			Destroy(gameObjectToDestroy);
		}
	}
}
