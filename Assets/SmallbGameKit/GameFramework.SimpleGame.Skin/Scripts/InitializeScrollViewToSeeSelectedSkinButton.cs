using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

using UniSkin;

namespace GameFramework.SimpleGame.Skin
{
	[AddComponentMenu("GameFramework/SimpleGame/Skin/InitializeScrollViewToSeeSelectedSkinButton")]
	public class InitializeScrollViewToSeeSelectedSkinButton : MonoBehaviour
	{
		public ScrollRect scrollRect;

		public SkinSelectorsSpawner skinSelectorsSpanwer;

		bool firstUpdate = true;

		Transform Target
		{
			get
			{
				foreach(SkinSelector selector in skinSelectorsSpanwer.SkinSelectors)
				{
					if(selector.SkinSelected)
					{
						return selector.transform;
					}
				}
				return null;
			}
		}

		void OnEnable()
		{
			firstUpdate = true;
		}

		void LateUpdate()
		{
			if(firstUpdate)
			{
				FirstUpdate();
				firstUpdate = false;
			}
		}

		void FirstUpdate()
		{
			Transform target = Target;

			if(target == null)
				return;

			RectTransform content = scrollRect.content;

			float offset = content.parent.InverseTransformPoint(target.position).y;
			content.localPosition -= offset * Vector3.up;
			float normalizedPosition = scrollRect.verticalNormalizedPosition;
			//Debug.Log(offset + ", " + normalizedPosition);

			if(normalizedPosition < 0.0f)
			{
				scrollRect.verticalNormalizedPosition = 0.0f;
			}
			else if(normalizedPosition > 1.0f)
			{
				scrollRect.verticalNormalizedPosition = 1.0f;
			}
		}
	}
}