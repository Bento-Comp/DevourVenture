using UnityEngine;

using UnityEngine.UI;

namespace UniFillBar
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniFillBar/FillValueVisual_ImageFill")]
	public class FillValueVisual_ImageFill : FillValueVisualBase
	{
		public Image image;

		protected override void OnSetFillAmount(float fillAmount)
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(image == null)
					return;
			}
			#endif

			image.fillAmount = fillAmount;
		}
	}
}