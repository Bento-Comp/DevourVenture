using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensor_Cast_Ray")]
	public abstract class PhysicsSensor_Cast_Ray : PhysicsSensor_Cast_Base
	{
		protected override int Cast(Vector3 origin, Vector3 direction, float length,
			int layerMask, QueryTriggerInteraction queryTriggerInteraction, ref RaycastHit[] hitBuffer)
		{
			return Physics.RaycastNonAlloc(
				origin,
				direction,
				hitBuffer,
				length,
				layerMask,
				queryTriggerInteraction);
		}
    }
}