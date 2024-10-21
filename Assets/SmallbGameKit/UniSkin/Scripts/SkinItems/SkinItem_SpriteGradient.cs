using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_SpriteGradient")]
	public class SkinItem_SpriteGradient : SkinItem_SpriteBase
	{
		public enum EGradientType
		{
			ScaledWithCount,
			FixedMax
		}

		public EGradientType gradientType = EGradientType.ScaledWithCount;

		[SerializeField]
		List<Sprite> sprites = new List<Sprite>();

		public int maxIndex = 20;

		public override Sprite GetSprite(int index = 0, int count = 1)
		{
			int max = 0;
			switch(gradientType)
			{
			case EGradientType.ScaledWithCount:
				{
					max = count - 1;
				}
				break;

			case EGradientType.FixedMax:
				{
					max = maxIndex;
				}
				break;
			}

			if(max <= 0)
				max = 0;

			if(sprites.Count <= 0)
				return null;

			if(sprites.Count == 1)
				return sprites[0];

			float totalPercent = Mathf.Clamp01(Mathf.InverseLerp(0, max, index));

			float indexPercent = totalPercent * (sprites.Count - 1);

			int indexInt = Mathf.FloorToInt(indexPercent);

			if(indexInt >= sprites.Count - 1)
			{
				return sprites[sprites.Count - 1];
			}

			float betweenPercent = indexPercent - indexInt;

			if(betweenPercent < 0.5f)
			{
				return sprites[indexInt];
			}
			else
			{
				return sprites[indexInt + 1];
			}
		}
	}
}
