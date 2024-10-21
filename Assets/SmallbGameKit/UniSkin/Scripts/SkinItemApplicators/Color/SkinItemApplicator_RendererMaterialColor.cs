using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_RendererMaterialColor")]
	public class SkinItemApplicator_RendererMaterialColor : SkinItemApplicator_ColorBase
    {
		public Renderer rendererComponent;

		protected override Color CurrentColor
		{
			get
			{
				Material material = rendererComponent.sharedMaterial;

				if(material == null)
					return Color.white;

				return material.color;
			}
		}

		protected override void OnColorChange(Color color)
		{
			Material material = rendererComponent.material;
            if(material != null)
			{
			    material.color = color;
			}
		}
	}
}
