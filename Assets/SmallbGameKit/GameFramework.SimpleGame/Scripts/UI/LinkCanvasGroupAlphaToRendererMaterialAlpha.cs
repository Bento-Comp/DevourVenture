using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(32000)]
	[AddComponentMenu("GameFramework/SimpleGame/LinkCanvasGroupAlphaToRendererMaterialAlpha")]
	public class LinkCanvasGroupAlphaToRendererMaterialAlpha : MonoBehaviour
	{
		public CanvasGroup canvasGroupFrom;

		public List<Renderer> renderers;

		void LateUpdate()
		{
			UpdateLink();
		}

		void UpdateLink()
		{
			foreach(Renderer rendererComponent in renderers)
			{
				Material material = rendererComponent.material;

				Color color = material.color;

				color.a = canvasGroupFrom.alpha;

				material.color = color;
			}
		}
	}
}