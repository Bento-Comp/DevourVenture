using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

namespace UniButton
{
	[Serializable]
	public class TouchColliderRaycastParameters
	{
		public LayerMask layerMask;
		public bool firstHitOnly;
		public bool useMaxDistance;
		public float maxDistance;

		public float MaxDistance => useMaxDistance ? maxDistance : float.PositiveInfinity;

		public override bool Equals(object otherObject) =>
		otherObject is TouchColliderRaycastParameters other &&
			(other.layerMask, other.firstHitOnly, other.useMaxDistance, other.maxDistance)
			.Equals((layerMask, firstHitOnly, useMaxDistance, maxDistance));

		public override int GetHashCode() => (layerMask, firstHitOnly, useMaxDistance, maxDistance).GetHashCode();
	}
}