using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_String")]
	public class SkinItem_String : SkinItem_StringBase
	{
		[SerializeField]
		string value = "";

		public override string GetString(int index = 0, int count = 1)
		{
			return value;
		}
	}
}
