using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_ColorBase")]
	public abstract class SkinItem_ColorBase : SkinItemBase
	{
		public abstract Color GetColor(int index = 0, int count = 1);
	}
}
