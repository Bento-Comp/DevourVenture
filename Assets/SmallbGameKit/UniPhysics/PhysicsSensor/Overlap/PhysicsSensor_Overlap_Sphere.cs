using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensor_SphereOverlap")]
	public abstract class PhysicsSensor_SphereOverlap : PhysicsSensor_Overlap_Base
	{
		public float radius = 1.0f;

		public Vector3 SphereCenter => transform.position;

		public override void Overlap(int layerMask, Collider[] colliderBuffer, QueryTriggerInteraction queryTriggerInteraction)
		{
			int overlapCount = Physics.OverlapSphereNonAlloc(
				SphereCenter,
				radius,
				colliderBuffer,
				layerMask,
				queryTriggerInteraction);

			for(int i = 0; i < overlapCount; ++i)
			{
				OnOverlapCollider(colliderBuffer[i]);
			}
		}

        void OnDrawGizmos()
        {
			Gizmos.color = Color.green;

			Gizmos.DrawWireSphere(SphereCenter, radius);
        }
    }
}