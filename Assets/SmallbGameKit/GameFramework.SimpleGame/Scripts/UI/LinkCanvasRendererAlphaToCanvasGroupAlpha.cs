using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/LinkCanvasRendererAlphaToCanvasGroupAlpha")]
	public class LinkCanvasRendererAlphaToCanvasGroupAlpha : MonoBehaviour
	{
		public CanvasRenderer canvasFrom;

		public CanvasGroup canvasGroupTo;

		public float scale = 1.0f;

		void LateUpdate()
		{
			UpdateLink();
		}

		void UpdateLink()
		{
			canvasGroupTo.alpha = 1.0f - (1.0f - canvasFrom.GetAlpha()) * scale;
		}
	}
}