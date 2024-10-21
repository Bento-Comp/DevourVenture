using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;

namespace UniMoney
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("UniMoney/MoneyCounter_ValueText_LayoutRebuilderOnTextChange")]
	public class MoneyCounter_ValueText_LayoutRebuilderOnTextChange : MonoBehaviour 
	{
		public MoneyCounter_ValueText_Base valueText;

		public RectTransform rectTransformToRebuild;

		bool rebuildInProgress;

		void OnEnable()
		{
			valueText.onTextChange += OnTextChange;
		}

		void OnDisable()
		{
			valueText.onTextChange -= OnTextChange;
		}

		void OnTextChange()
		{
			if(rebuildInProgress)
					return;

			rebuildInProgress = true;
			rectTransformToRebuild.gameObject.SetActive(false);
			rectTransformToRebuild.gameObject.SetActive(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransformToRebuild);
			rebuildInProgress = false;
		}
	}
}