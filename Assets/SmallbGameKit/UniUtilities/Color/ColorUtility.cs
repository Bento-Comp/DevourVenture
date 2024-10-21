using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniUtilities
{
	public class ColorUtility
	{
		public static Color LerpRGB(Color a, Color b, float t)
		{
			return Color.Lerp(a, b, t);
		}

		public static Color LerpHSB(Color a, Color b, float t)
		{
			HSBColor a_hsb = new HSBColor(a);
			HSBColor b_hsb = new HSBColor(b);

			HSBColor lerped_hsb = HSBColor.Lerp(a_hsb, b_hsb, t);

			return lerped_hsb.ToColor();
		}
	}
}
