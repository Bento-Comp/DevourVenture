using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame.MoneyUIGiverInternal
{
	[AddComponentMenu("GameFramework/SimpleGame/GivenItem_GoToTarget")]
	public class GivenItem_GoToTarget : MonoBehaviour
	{
		public System.Action onTargetReached;

		public GivenItem givenItem;

		public float attractionForce = 10.0f;

		public float targetReachedRadius = 0.5f;

		void Start()
		{
			StartCoroutine(GoToTargetCoroutine());
		}

		IEnumerator GoToTargetCoroutine()
		{
			float currentVelocity = 0.0f;

			while(true)
			{
				Vector2 target = givenItem.target.TargetPosition;
				Vector2 position = givenItem.body.Position;

				currentVelocity += attractionForce * Time.deltaTime;
				float movementLength = currentVelocity * Time.deltaTime;

				if(IsMovementCrossingTarget(position, movementLength, target, targetReachedRadius))
					break;

				givenItem.body.Position = position + movementLength * (target - position).normalized;

				yield return null;
			}

			givenItem.body.Position = givenItem.target.TargetPosition;
			onTargetReached?.Invoke();
		}

		bool IsMovementCrossingTarget(Vector2 start, float movementLength, Vector2 target, float reachTargetRadius)
		{
			float distanceToTarget = Vector2.Distance(start, target);
			return movementLength >= distanceToTarget - reachTargetRadius;
		}
	}
}