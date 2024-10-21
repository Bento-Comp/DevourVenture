using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniInterCollision
{
	// Hidden component, made to be added at runtime
	[AddComponentMenu("")]
	public class IgnoreCollisionForDuration : MonoBehaviour
	{
		public Collider colliderA;
		public Collider colliderB;

		public float duration;

		bool ignoreCollisionStarted;

		bool ignoreCollisionEnded;

		public static IgnoreCollisionForDuration StartIgnoreCollisionForDuration(Collider hostCollider, Collider colliderToIgnore,
			float duration)
		{
			IgnoreCollisionForDuration ignoreCollisionInstance = hostCollider.gameObject.AddComponent<IgnoreCollisionForDuration>();

			ignoreCollisionInstance.colliderA = hostCollider;
			ignoreCollisionInstance.colliderB = colliderToIgnore;

			ignoreCollisionInstance.duration = duration;

			ignoreCollisionInstance.StartToIgnoreCollision();

			return ignoreCollisionInstance;
		}

        void StartToIgnoreCollision()
		{
			if(ignoreCollisionStarted)
				return;

			ignoreCollisionStarted = true;

			IgnoreCollisionBegin();

			StartCoroutine(IgnoreCollisionEndAfterDelay(duration));
		}

		IEnumerator IgnoreCollisionEndAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			IgnoreCollisionEnd();
		}

		void IgnoreCollisionBegin()
		{
			Physics.IgnoreCollision(colliderA, colliderB, true);
		}

		void IgnoreCollisionEnd()
		{
			if(ignoreCollisionEnded)
				return;

			ignoreCollisionEnded = true;

			Physics.IgnoreCollision(colliderA, colliderB, false);

			Destroy(this);
		}
	}
}