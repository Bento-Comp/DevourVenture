using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Sprite_ByPlatform_Preprocessor")]
	public class SkinItem_Sprite_ByPlatform_Preprocessor : SkinItem_SpriteBase
	{
		public Sprite sprite_default;

		public Sprite sprite_ios;

		public Sprite sprite_android;

		public override Sprite GetSprite(int index = 0, int count = 1)
		{
			#if UNITY_ANDROID
			return sprite_android;
			#elif UNITY_IOS
			return sprite_ios;
			#else
			return sprite_default;
			#endif
		}
	}
}
