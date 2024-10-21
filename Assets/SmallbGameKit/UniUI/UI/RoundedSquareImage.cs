using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UniUI
{
	[ExecuteAlways]
	[AddComponentMenu("UniUI/RoundedSquareImage")]
	public class RoundedSquareImage : UIBehaviour
	{
		public List<RectTransform> controlledTransforms = new List<RectTransform>();

		public Sprite sprite;

		[Range(0.0001f, 1.0f)]
		public float roundedScale = 1.0f;

		RectTransform rectTransform;

		Vector2 RoundedScaleVector
		{
			get
			{
				return roundedScale * Vector3.one;
			}
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			UpdateScale();
		}

		#if UNITY_EDITOR
		protected override void OnValidate()
        {
			UpdateScale();
        }
		#endif

		void UpdateScale()
		{
			if(rectTransform == null)
			{
				rectTransform = GetComponent<RectTransform>();

				if(rectTransform == null)
					return;
			}

			foreach(RectTransform controlledTransform in controlledTransforms)
				UpdateScale(controlledTransform);
		}

		void UpdateScale(RectTransform controlledTransform)
		{
			if(controlledTransform == null)
				return;

			if(sprite == null)
				return;

			Vector2 parentSize = rectTransform.rect.size;

			if(parentSize.x == 0.0f)
				return;

			if(parentSize.y == 0.0f)
				return;

			Vector2 originalImageSize = sprite.rect.size;

			Vector2 parentResize;
			parentResize.x = parentSize.x / originalImageSize.x;
			parentResize.y = parentSize.y / originalImageSize.y;

			Vector2 roundedScaleVector = RoundedScaleVector;

			if(parentSize.x >= parentSize.y)
			{
				roundedScaleVector.x *= parentSize.y / parentSize.x;
			}
			else
			{
				roundedScaleVector.y *= parentSize.x / parentSize.y;
			}

			roundedScaleVector.x *= parentResize.x;
			roundedScaleVector.y *= parentResize.y;

			Vector3 scale = controlledTransform.localScale;
			scale.x = roundedScaleVector.x;
			scale.y = roundedScaleVector.y;
			controlledTransform.localScale = scale;

			Vector2 size = new Vector2(parentSize.x / roundedScaleVector.x, parentSize.y / roundedScaleVector.y);
			controlledTransform.sizeDelta = size;
		}
	}
}