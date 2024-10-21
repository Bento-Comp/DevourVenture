using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Float")]
	public class SkinItem_Float : SkinItem_FloatBase
	{
		[SerializeField]
		float value = 0.0f;

		public override float GetFloat(int index = 0, int count = 1)
		{
			return value;
		}
	}
}
