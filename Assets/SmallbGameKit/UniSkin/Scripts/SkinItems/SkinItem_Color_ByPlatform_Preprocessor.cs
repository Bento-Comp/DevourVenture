using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Color_ByPlatform_Preprocessor")]
	public class SkinItem_Color_ByPlatform_Preprocessor : SkinItem_ColorBase
	{
		public Color color_default = Color.white;

		public Color color_ios = Color.white;

		public Color color_android = Color.white;

		public override Color GetColor(int index = 0, int count = 1)
		{
			#if UNITY_ANDROID
			return color_android;
			#elif UNITY_IOS
			return color_ios;
			#else
			return color_default;
			#endif
		}
	}
}
