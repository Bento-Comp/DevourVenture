using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItem_ColorGradient")]
	public class SkinItem_ColorGradient : SkinItem_ColorBase
	{
		public enum EGradientType
		{
			ScaledWithCount,
			FixedMax
		}

		public EGradientType gradientType = EGradientType.ScaledWithCount;

		public bool interpolateColor = true;

		[SerializeField]
		List<Color> colors = new List<Color>{Color.white};

		public int maxIndex = 20;

		public override Color GetColor(int index = 0, int count = 1)
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

			if(colors.Count <= 0)
				return Color.white;

			if(colors.Count == 1)
				return colors[0];

			float totalPercent = Mathf.Clamp01(Mathf.InverseLerp(0, max, index));

			float indexPercent = totalPercent * (colors.Count - 1);

			int indexInt = Mathf.FloorToInt(indexPercent);

			if(indexInt >= colors.Count - 1)
			{
				return colors[colors.Count - 1];
			}

			float betweenPercent = indexPercent - indexInt;

			if(interpolateColor)
			{
				return Color.Lerp(colors[indexInt], colors[indexInt + 1], betweenPercent);
			}
			else
			{
				if(betweenPercent < 0.5f)
				{
					return colors[indexInt];
				}
				else
				{
					return colors[indexInt + 1];
				}
			}
		}
	}
}
