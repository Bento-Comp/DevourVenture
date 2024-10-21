using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Sprite")]
	public abstract class SkinItem_SpriteBase : SkinItemBase
	{
		public abstract Sprite GetSprite(int index = 0, int count = 1);
	}
}
