using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[ExecuteInEditMode()]
	[DefaultExecutionOrder(10)]
	[AddComponentMenu("UniConstraint/UniformPlanarAndHeightScale")]
	public class UniformPlanarAndHeightScale : TransformConstraint_Base
	{
		public float planarScale = 1.0f;

		public float heightScale = 1.0f;

		protected override void UpdateConstraint(Transform controlledTransform)
		{
			controlledTransform.localScale = new Vector3(planarScale, heightScale, planarScale);
		}

		void LateUpdate()
		{
#if UNITY_EDITOR
			Editor_Update();

			if(Application.isPlaying == false)
				return;
#endif

			UpdateConstraint();
		}
	}
}
