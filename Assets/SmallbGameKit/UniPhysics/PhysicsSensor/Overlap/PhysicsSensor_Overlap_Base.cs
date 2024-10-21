using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensor_Overlap_Base")]
	public abstract class PhysicsSensor_Overlap_Base : PhysicsSensor_Base
	{
		public abstract void Overlap(int layerMask, Collider[] colliderBuffer, QueryTriggerInteraction queryTriggerInteraction);

		protected abstract void OnOverlapCollider(Collider other);

		protected override void Register(PhysicsSensorManager manager)
		{
			manager.GetSensorLayer<PhysicsSensorLayer_Overlap>(SensorLayerName).Add(this);
		}

		protected override void Unregister(PhysicsSensorManager manager)
		{
			manager.GetSensorLayer<PhysicsSensorLayer_Overlap>(SensorLayerName).Remove(this);
		}
	}
}