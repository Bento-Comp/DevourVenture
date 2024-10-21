using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_TransformUniformScale")]
	public class SkinItemApplicator_TransformUniformScale : SkinItemApplicator_FloatBase
	{
		protected override void OnFloatChange(float value)
		{
			transform.localScale = Vector3.one * value;
		}
	}
}
