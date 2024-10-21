using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensor_Cast_Base")]
	public abstract class PhysicsSensor_Cast_Base : PhysicsSensor_Base
	{
		public Vector3 Position => transform.position;

		public Vector3 CastDirection { get; private set; } = Vector3.zero;

		Vector3 lastPosition;

		HashSet<Collider> hitColliders;

		public void UpdateCast(int layerMask, RaycastHit[] hitBuffer, QueryTriggerInteraction queryTriggerInteraction, bool alsoCastInReverseDirection)
		{
			Vector3 currentPosition = Position;
			Vector3 direction = currentPosition - lastPosition;

			float length = direction.magnitude;

			if(length <= 0.0f)
			{
				CastDirection = Vector3.zero;
				lastPosition = currentPosition;
				return;
			}

			direction /= length;

			CastDirection = direction;

			int hitCount = Cast(lastPosition, direction, length,
				layerMask, queryTriggerInteraction, ref hitBuffer);

			if(alsoCastInReverseDirection)
			{
				if(hitColliders == null)
				{
					hitColliders = new HashSet<Collider>();
				}
				else
				{
					hitColliders.Clear();
				}
			}

			for(int i = 0; i < hitCount; ++i)
			{
				Collider collider = hitBuffer[i].collider;

				OnHitCollider(collider);

				if(alsoCastInReverseDirection)
				{
					hitColliders.Add(collider);
				}
			}

			if(alsoCastInReverseDirection)
			{
				hitCount = Cast(currentPosition, -direction, length,
				layerMask, queryTriggerInteraction, ref hitBuffer);

				for(int i = hitCount - 1; i >= 0 ; --i)
				{
					Collider collider = hitBuffer[i].collider;

					if(hitColliders.Contains(collider))
						continue;

					OnHitCollider(collider);
				}
			}

			lastPosition = currentPosition;
		}

		protected override void Initialise()
		{
			lastPosition = Position;
		}

		protected abstract int Cast(Vector3 origin, Vector3 direction, float length,
			int layerMask, QueryTriggerInteraction queryTriggerInteraction, ref RaycastHit[] hitBuffer);

		protected abstract void OnHitCollider(Collider other);

		protected override void Register(PhysicsSensorManager manager)
		{
			manager.GetSensorLayer<PhysicsSensorLayer_Cast>(SensorLayerName).Add(this);
		}

		protected override void Unregister(PhysicsSensorManager manager)
		{
			manager.GetSensorLayer<PhysicsSensorLayer_Cast>(SensorLayerName).Remove(this);
		}
	}
}