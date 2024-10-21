using UnityEngine;

namespace UniFillBar
{
	[AddComponentMenu("UniFillBar/FillValue_Remapped")]
	public class FillValue_Remapped : FillValue
	{
		public AnimationCurve fillAmountRemapping = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

		public override float FillAmount
		{
			get => RemapFillAmount(base.FillAmount);

			protected set => base.FillAmount = value;
		}

		public override float GlobalFillAmount
		{
			get => RemapFillAmount(base.GlobalFillAmount);

			protected set => base.GlobalFillAmount = value;
		}

		float RemapFillAmount(float fillAmountToRemap)
		{
			return fillAmountRemapping.Evaluate(fillAmountToRemap);
		}
	}
}