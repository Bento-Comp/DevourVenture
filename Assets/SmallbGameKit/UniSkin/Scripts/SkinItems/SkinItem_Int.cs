using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Int")]
	public class SkinItem_Int : SkinItem_IntBase
	{
		[SerializeField]
		int value = 0;

		public override int GetInt(int index = 0, int count = 1)
		{
			return value;
		}
	}
}
