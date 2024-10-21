using UnityEngine;

namespace UniFillBar
{
	[AddComponentMenu("UniFillBar/FillValue")]
	public class FillValue : FillValueBase
	{
		[SerializeField]
		[Range(0.0f, 1.0f)]
		float fillAmount = 1.0f;

		bool globalFillAmountSet;
		float globalFillAmount = 1.0f;

		public override float FillAmount
		{
			get
			{
				return fillAmount;
			}

			protected set
			{
				fillAmount = value;
				if(globalFillAmountSet == false)
				{
					GlobalFillAmount = value;
				}
			}
		}

		public override float GlobalFillAmount
		{
			get => globalFillAmountSet ? globalFillAmount : fillAmount;

			protected set
			{
				globalFillAmount = value;
				globalFillAmountSet = true;
			}
		}
	}
}