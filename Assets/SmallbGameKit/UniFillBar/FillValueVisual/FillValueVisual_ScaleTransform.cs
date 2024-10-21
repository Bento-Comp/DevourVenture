using UnityEngine;

namespace UniFillBar
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniFillBar/FillValueVisual_ScaleTransform")]
	public class FillValueVisual_ScaleTransform : FillValueVisualBase
	{
		public Transform fillTransform;
		public bool freezeX;
		public bool freezeY;

		protected override void OnSetFillAmount(float fillAmount)
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(fillTransform == null)
					return;
			}
			#endif

			Vector3 scale = fillTransform.localScale;

			if(freezeX == false)
				scale.x = fillAmount;

			if(freezeY == false)
				scale.y = fillAmount;

			fillTransform.localScale = scale;
		}
	}
}