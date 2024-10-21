using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniCurves
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniCurves/BezierCurve_PlaceTransforms")]
	public class BezierCurve_PlaceTransforms : MonoBehaviour
	{
		public enum EPlacementMode
		{
			Percent,
			Spacing
		}

		[System.Serializable]
		public class SpacingInfos
		{
			public float spacing = 1.0f;

			public float step = 0.01f;
		}
		
		public bool autoFill = true;

		public BezierCurve bezierCurve;

		public EPlacementMode placementMode;

		public SpacingInfos spacing;

		public List<Transform> transforms = new List<Transform>();

		[SerializeField]
		List<float> percents = new List<float>();

		public float GetPercent(int index)
		{
			//index = Mathf.Clamp(index, percents.Count - 1);

			return percents[index];
		}

		void Update()
		{
			if(Application.isPlaying)
				return;

			if(bezierCurve == null)
				return;

			if(autoFill)
			{
				transforms.Clear();
				foreach(Transform child in transform)
				{
					transforms.Add(child);
				}
			}

			percents.Clear();

			switch(placementMode)
			{
			case EPlacementMode.Percent:
				{
					UpdatePlacement_Percent();
				}
				break;

			case EPlacementMode.Spacing:
				{
					UpdatePlacement_Spacing();
				}
				break;
			}
		}

		void UpdatePlacement_Percent()
		{
			int index = 0;
			int count = transforms.Count;
			foreach(Transform transform in transforms)
			{
				float percent = (float)index / (float)(count - 1);
				transform.position = bezierCurve.GetPoint(percent);

				percents.Add(percent);

				++index;
			}
		}

		void UpdatePlacement_Spacing()
		{
			if(transforms.Count <= 0)
				return;
			
			float percent = 0.0f;
			transforms[0].position = bezierCurve.GetPoint(0.0f);
			percents.Add(0.0f);
			for(int i = 1; i < transforms.Count; ++i)
			{
				transforms[i].position = bezierCurve.GetPointAlongTheCurve(percent, spacing.spacing, out percent, spacing.step);
				percents.Add(percent);
			}
		}
	}
}