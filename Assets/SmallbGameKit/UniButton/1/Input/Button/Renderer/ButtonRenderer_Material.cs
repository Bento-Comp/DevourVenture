using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_Material")]
	public class ButtonRenderer_Material : ButtonRenderer
	{
		public Renderer buttonRenderer;
		
		public Material up;
		
		public Material down;
		
		protected override void SetUp()
		{
			buttonRenderer.sharedMaterial = up;
		}
		
		protected override void SetDown()
		{
			buttonRenderer.sharedMaterial = down;
		}
	}
}