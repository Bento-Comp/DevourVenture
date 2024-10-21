using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniInterCollision
{
	[AddComponentMenu("UniInterCollision/SmoothDepenetration")]
	public class SmoothDepenetration : MonoBehaviour
	{
		public Rigidbody rigidbodyComponent;

		public float duration = 1.0f;
		public float maxDepenetrationVelocity = 0.1f;

		public static SmoothDepenetration StartSmoothDepenetration(Rigidbody rigidbodyComponent,
			float duration, float maxDepenetrationVelocity)
		{
			SmoothDepenetration smoothDepenetration = CreateSmoothDepenetration(rigidbodyComponent);

			smoothDepenetration.duration = duration;
			smoothDepenetration.maxDepenetrationVelocity = maxDepenetrationVelocity;

			smoothDepenetration.Initialize();

			return smoothDepenetration;
		}

		 static SmoothDepenetration CreateSmoothDepenetration(Rigidbody rigidbodyComponent)
		{
			SmoothDepenetration smoothDepenetration = rigidbodyComponent.gameObject.AddComponent<SmoothDepenetration>();

			smoothDepenetration.rigidbodyComponent = rigidbodyComponent;

			return smoothDepenetration;
		}

		void Initialize()
		{
			StartCoroutine(SmoothDepenetrationCoroutine());
		}

		IEnumerator SmoothDepenetrationCoroutine()
		{
			float maxDepenetrationVelocity_save = rigidbodyComponent.maxDepenetrationVelocity;

			rigidbodyComponent.maxDepenetrationVelocity = maxDepenetrationVelocity;

			yield return new WaitForSeconds(duration);

			rigidbodyComponent.maxDepenetrationVelocity = maxDepenetrationVelocity_save;
		}
	}
}