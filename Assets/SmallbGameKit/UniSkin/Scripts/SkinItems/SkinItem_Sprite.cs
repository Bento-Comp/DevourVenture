using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Sprite")]
	public class SkinItem_Sprite : SkinItem_SpriteBase
	{
		[SerializeField]
		Sprite sprite = null;

		public override Sprite GetSprite(int index = 0, int count = 1)
		{
			return sprite;
		}
	}
}
