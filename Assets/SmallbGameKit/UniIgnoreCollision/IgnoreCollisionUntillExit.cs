using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniInterCollision
{
	[AddComponentMenu("UniInterCollision/IgnoreCollisionUntillExit")]
	public class IgnoreCollisionUntillExit : MonoBehaviour
	{
		public Collider colliderSpawner;
		public Collider colliderSpawnee;

		public bool useMaxDuration;
		public float maxDuration;

		bool end;

		public static IgnoreCollisionUntillExit StartIgnoreCollisionUntilExist(Collider colliderSpawner, Collider colliderSpawnee,
			float maxDuration)
		{
			IgnoreCollisionUntillExit ignoreCollisionInstance = CreateIgnoreCollisionUntilExist(colliderSpawner, colliderSpawnee);

			ignoreCollisionInstance.useMaxDuration = true;
			ignoreCollisionInstance.maxDuration = maxDuration;

			ignoreCollisionInstance.Initialize();

			return ignoreCollisionInstance;
		}

		public static IgnoreCollisionUntillExit StartIgnoreCollisionUntilExist(Collider colliderSpawner, Collider colliderSpawnee)
		{
			IgnoreCollisionUntillExit ignoreCollisionInstance = CreateIgnoreCollisionUntilExist(colliderSpawner, colliderSpawnee);

			ignoreCollisionInstance.Initialize();

			return ignoreCollisionInstance;
		}

		static IgnoreCollisionUntillExit CreateIgnoreCollisionUntilExist(Collider colliderSpawner, Collider colliderSpawnee)
		{
			GameObject ignoreCollisionGameObject = new GameObject();
			IgnoreCollisionUntillExit ignoreCollisionInstance = ignoreCollisionGameObject.AddComponent<IgnoreCollisionUntillExit>();

			ignoreCollisionInstance.colliderSpawner = colliderSpawner;
			ignoreCollisionInstance.colliderSpawnee = colliderSpawnee;

			return ignoreCollisionInstance;
		}

		void Initialize()
		{
			IgnoreCollisionBegin();

			gameObject.name = "IgnoreCollisionUntilExit_" + colliderSpawner.name + "_" + colliderSpawnee.name;
			gameObject.layer = colliderSpawnee.gameObject.layer;
			gameObject.transform.SetParent(colliderSpawnee.transform, false);

			Collider exitFromSpawneeTrigger = gameObject.AddComponent<Collider>(colliderSpawnee);
			exitFromSpawneeTrigger.isTrigger = true;

			if(useMaxDuration)
			{
				StartCoroutine(IgnoreCollisionEndAfterDelay(maxDuration));
			}
		}

		void OnTriggerExit(Collider other)
		{
			if(other != colliderSpawner)
				return;

			IgnoreCollisionEnd();
		}

		IEnumerator IgnoreCollisionEndAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			IgnoreCollisionEnd();
		}

		void IgnoreCollisionBegin()
		{
			Physics.IgnoreCollision(colliderSpawner, colliderSpawnee, true);
		}

		void IgnoreCollisionEnd()
		{
			if(end)
				return;

			end = true;

			Physics.IgnoreCollision(colliderSpawner, colliderSpawnee, false);
			Destroy(gameObject);
		}
	}
}