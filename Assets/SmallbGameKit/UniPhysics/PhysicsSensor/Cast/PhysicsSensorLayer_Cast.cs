using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensorLayer_Cast")]
	public class PhysicsSensorLayer_Cast : PhysicsSensorLayer_Generic<PhysicsSensor_Cast_Base>
	{
		public bool alsoCastInReverseDirection = true;

		RaycastHit[] hitBuffer;

		protected override void Initialise()
		{
			hitBuffer = new RaycastHit[maxColliderBySensorUpdate];
		}

		protected override void UpdateSensor(PhysicsSensor_Cast_Base sensor)
		{
			sensor.UpdateCast(LayerMask, hitBuffer, queryTriggerInteraction, alsoCastInReverseDirection);
		}
	}
}