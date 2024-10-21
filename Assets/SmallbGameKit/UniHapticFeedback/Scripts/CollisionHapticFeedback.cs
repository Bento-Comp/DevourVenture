using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

namespace UniHapticFeedback
{
	[AddComponentMenu("UniHapticFeedback/CollisionHapticFeedback")]
	public class CollisionHapticFeedback : MonoBehaviour
	{
		[System.Serializable]
		public class FeedbackTypeByVelocity
		{
			public EHapticFeedbackType feedbackType = EHapticFeedbackType.Light;

			public float minVelocity = 0.0f;
			public float maxVelocity = float.PositiveInfinity;

			public bool IsInRange(float velocity)
			{
				return velocity >= minVelocity && velocity < maxVelocity;
			}
		}

		public bool active = false;

		public List<FeedbackTypeByVelocity> feedbackTypesByVelocity = new List<FeedbackTypeByVelocity>(){new FeedbackTypeByVelocity()};

		public bool excludeTag = false;
		public List<string> tags = new List<string>();

		public float minDurationBetweenTrigger = 0.0f;

		public float debug_collisionVelocity;

		public bool debug_logTriggerFeedback;

		bool canTrigger = true;

		float remaingTimeBeforeCanTriggerAgain;

		void Update()
		{
			if(canTrigger == false)
			{
				remaingTimeBeforeCanTriggerAgain -= Time.deltaTime;
				if(remaingTimeBeforeCanTriggerAgain <= 0.0f)
					canTrigger = true;
			}
		}

		void OnCollisionEnter(Collision collision)
		{
			if(active == false)
				return;

			Collider other = collision.collider;

			bool containsColliderTag = false;
			if(tags.Contains(other.tag))
			{
				containsColliderTag = true;
			}
			else if(other.attachedRigidbody != false && tags.Contains(other.attachedRigidbody.tag))
			{
				containsColliderTag = true;
			}

			if(
				(containsColliderTag && excludeTag)
			   ||
				(containsColliderTag == false && excludeTag == false)
			  )
				return;

			float collisionVelocity = collision.relativeVelocity.magnitude;

			debug_collisionVelocity = collisionVelocity;

			TriggerFeedbackOnCollision(collisionVelocity);
		}

		void TriggerFeedbackOnCollision(float collisionVelocity)
		{
			foreach(FeedbackTypeByVelocity feedbackTypeByVelocity in feedbackTypesByVelocity)
			{
				if(feedbackTypeByVelocity.IsInRange(collisionVelocity))
				{
					TryTriggerFeedback(feedbackTypeByVelocity.feedbackType);

					return;
				}
			}
		}

		void TryTriggerFeedback(EHapticFeedbackType feedbackType)
		{
			if(canTrigger == false)
				return;

			if(debug_logTriggerFeedback)
				Debug.Log("CollisionHapticFeedback : TryTriggerFeedback : " + feedbackType);

			HapticFeedbackManager.TriggerHapticFeedback(feedbackType);

			if(minDurationBetweenTrigger > 0.0f)
			{
				canTrigger = false;
				remaingTimeBeforeCanTriggerAgain = minDurationBetweenTrigger;
			}
		}
	}
}