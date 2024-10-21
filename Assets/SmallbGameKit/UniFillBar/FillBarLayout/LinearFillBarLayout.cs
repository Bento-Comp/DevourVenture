using UnityEngine;

namespace UniFillBar
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniFillBar/LinearFillBarLayout")]
	public class LinearFillBarLayout : FillBarLayoutBase
	{
		public Transform containersPivot;

		public float partSize = 0.15f;

		public float maxWidth = 1.8f;

		public bool stretchToFillWidth;

		public override void UpdateLayout()
		{
			Transform pivot = containersPivot;
			if(pivot == null)
				pivot = transform;

			int childrenCount = pivot.childCount;

			float barWidth = childrenCount * partSize;
			float widthScale = 1.0f;
			if(stretchToFillWidth || barWidth > maxWidth)
			{
				widthScale = maxWidth/barWidth;
			}

			float positionX =  (childrenCount - 1) * partSize * 0.5f * widthScale;
			// left side pivot to center pivot
			positionX += partSize * 0.5f * widthScale;


			foreach(Transform children in pivot)
			{
				children.localPosition = new Vector3(positionX, 0.0f, 0.0f);
				children.localScale = new Vector3(widthScale, 1.0f, 1.0f);
				positionX -= partSize * widthScale;
			}
		}
	}
}