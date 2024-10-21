using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/SelfDestroyByAnimator")]
	public class SelfDestroyByAnimator : MonoBehaviour
	{
		public GameObject rootToDestroy;

		public void StartSelfDestroy()
		{
			DoSelfDestroy();
		}

		void DoSelfDestroy()
		{
			Destroy(rootToDestroy);
		}
	}
}
