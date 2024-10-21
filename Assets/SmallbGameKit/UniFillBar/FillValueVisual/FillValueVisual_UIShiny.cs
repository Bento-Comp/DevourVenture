using UnityEngine;

using UnityEngine.UI;
using Coffee.UIExtensions;

namespace UniFillBar
{
	[AddComponentMenu("UniFillBar/FillValueVisual_UIShiny")]
	public class FillValueVisual_UIShiny : FillValueVisualBase
	{
		public UIShiny uiShiny;

		public bool resetFromStartOnPlay = false;

		float lastFillAmount;

		protected override void OnSetFillAmount(float fillAmount)
		{
#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
#endif
			float fillAmountChange = fillAmount - lastFillAmount;
			if(fillAmountChange > 0.0f)
			{
				uiShiny.Play(resetFromStartOnPlay);
			}

			lastFillAmount = fillAmount;
		}
	}
}