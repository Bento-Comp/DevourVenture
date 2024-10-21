using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame.MoneyUIGiverInternal
{
	[AddComponentMenu("GameFramework/SimpleGame/GivenItem_ExplodeFromSpawnCenter")]
	public class GivenItem_ExplodeFromSpawnCenter : MonoBehaviour
	{
		public GivenItem givenItem;

		public float explosionSpeed = 10.0f;

		public float explosionPercentCenter = 0.0f;
		public float explosionPercentBorder = 1.0f;
		
		public float explosionSpeedVariancePercent = 0.1f;
		public float drag = 10.0f;

		float ExplosionSpeed => explosionSpeed
			* (1.0f +
			Random.Range(-1.0f, 1.0f) * explosionSpeedVariancePercent * 0.5f);

		void Start()
		{
			StartCoroutine(ExplodeFromSpawnCenterCoroutine(ExplosionSpeed));
		}

		IEnumerator ExplodeFromSpawnCenterCoroutine(float explosionSpeed)
		{
			Vector2 distanceFromCenterInPercent = givenItem.spawnCenter.distanceFromCenterInPercent;
			float percentFromCenter = distanceFromCenterInPercent.magnitude;

			Vector2 explosionDirection;
			if(percentFromCenter == 0.0f)
			{
				 explosionDirection = UniRandom.RandomUtility.RandomValueOnCircle();
			}
			else
			{
				explosionDirection = distanceFromCenterInPercent/percentFromCenter;
			}

			float explosionPercent = Mathf.Lerp(explosionPercentCenter, explosionPercentBorder, percentFromCenter);
			Vector2 currentVelocity = explosionSpeed * explosionDirection * explosionPercent;

			while(true)
			{
				currentVelocity = UniUtilities.PhysicsUtility.ApplyDrag(currentVelocity, drag, Time.deltaTime);

				if(currentVelocity == Vector2.zero)
					break;

				givenItem.body.Position += currentVelocity * Time.deltaTime;

				yield return null;
			}
		}
	}
}