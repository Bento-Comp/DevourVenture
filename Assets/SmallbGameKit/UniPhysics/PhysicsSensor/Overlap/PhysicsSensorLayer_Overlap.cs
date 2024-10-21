using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensorLayer_Overlap")]
	public class PhysicsSensorLayer_Overlap : PhysicsSensorLayer_Generic<PhysicsSensor_Overlap_Base>
	{
		Collider[] colliderBuffer;

		protected override void Initialise()
		{
			colliderBuffer = new Collider[maxColliderBySensorUpdate];
		}

		protected override void UpdateSensor(PhysicsSensor_Overlap_Base sensor)
		{
			sensor.Overlap(LayerMask, colliderBuffer, queryTriggerInteraction);
		}
	}
}