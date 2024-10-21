using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_MaterialColor")]
	public class ButtonRenderer_MaterialColor : ButtonRenderer_Color
	{	
		public Renderer rendererComponent;

		protected override Color GetColor()
		{
			return rendererComponent.sharedMaterial.color;
		}

		protected override void SetColor(Color color)
		{
			rendererComponent.material.color = color;
		}
	}
}