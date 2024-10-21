using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_BoolBase")]
	public abstract class SkinItem_BoolBase : SkinItemBase
	{
		public abstract bool GetBool(int index = 0, int count = 1);
	}
}
