using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MoneyRewardValueText")]
	public class MoneyRewardValueText : MonoBehaviour
	{
		public Text label;

		public RectTransform priceTaglayoutRectTransform;

		bool rebuildInProgress;

		void OnEnable()
		{
			label.text = MoneyRewardManager.Instance.CurrentReward.ToString();

			if(priceTaglayoutRectTransform != null)
			{
				if(rebuildInProgress)
					return;

				rebuildInProgress = true;
				priceTaglayoutRectTransform.gameObject.SetActive(false);
				priceTaglayoutRectTransform.gameObject.SetActive(true);
				LayoutRebuilder.ForceRebuildLayoutImmediate(priceTaglayoutRectTransform);
				rebuildInProgress = false;
			}
		}
	}
}
