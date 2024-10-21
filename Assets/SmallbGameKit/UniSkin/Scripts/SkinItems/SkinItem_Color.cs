using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_Color")]
	public class SkinItem_Color : SkinItem_ColorBase
	{
		[SerializeField]
		Color color = Color.white;

		public void SetColor(Color color)
		{
			this.color = color;
		}

		public override Color GetColor(int index = 0, int count = 1)
		{
			return color;
		}
	}
}
