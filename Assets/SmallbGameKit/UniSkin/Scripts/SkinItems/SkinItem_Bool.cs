using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Bool")]
	public class SkinItem_Bool : SkinItem_BoolBase
	{
		[SerializeField]
		bool value = false;

		public override bool GetBool(int index = 0, int count = 1)
		{
			return value;
		}
	}
}
