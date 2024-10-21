using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(32000)]
	[AddComponentMenu("GameFramework/SimpleGame/LinkCanvasRendererAlphaToRendererMaterialAlpha")]
	public class LinkCanvasRendererAlphaToRendererMaterialAlpha : MonoBehaviour
	{
		public CanvasRenderer canvasRendererFrom;

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

				color.a = canvasRendererFrom.GetInheritedAlpha() * canvasRendererFrom.GetAlpha();

				material.color = color;
			}
		}
	}
}