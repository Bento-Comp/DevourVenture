using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_TransformOffsetScale")]
	public class SkinItemApplicator_TransformLocalOffsetScale : SkinItemApplicator_FloatBase
	{
		public Vector3 offset;
		
		protected override void OnFloatChange(float value)
		{
			transform.localPosition = offset * value;
		}
	}
}
