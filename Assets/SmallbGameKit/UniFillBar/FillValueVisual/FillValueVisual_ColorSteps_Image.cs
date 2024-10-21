using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace UniFillBar
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniFillBar/FillValueVisual_ColorSteps_Image")]
	public class FillValueVisual_ColorSteps_Image : FillValueVisualBase
	{
		[Serializable]
		public class ColorStep : IComparable
		{
			public float fillAmountEnd = 1.0f;
			public Color color;

			 public int CompareTo(object obj)
			{
				if(obj == null)
					return 1;

				ColorStep colorStep = obj as ColorStep;

				if(colorStep == null)
					return 1;

				return fillAmountEnd.CompareTo(colorStep.fillAmountEnd);
			}
		}

		public List<Image> images;

		public List<ColorStep> colorSteps;

		protected override void OnSetFillAmount(float fillAmount)
		{
			colorSteps.Sort();

			foreach(ColorStep colorStep in colorSteps)
			{
				if(colorStep.fillAmountEnd >= fillAmount)
				{
					ApplyColor(colorStep.color);
						return;
				}
			}
		}

		void ApplyColor(Color color)
		{
			foreach(Image image in images)
				image.color = color;
		}
	}
}