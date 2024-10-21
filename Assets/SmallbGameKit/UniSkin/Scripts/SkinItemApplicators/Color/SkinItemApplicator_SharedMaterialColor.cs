using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_SharedMaterialColor")]
	public class SkinItemApplicator_SharedMaterialColor : SkinItemApplicator_ColorBase
    {
		public Material material;

		protected override Color CurrentColor
		{
			get
			{
				if(material == null)
					return Color.white;

				return material.color;
			}
		}

		protected override void OnColorChange(Color color)
		{
            if(material != null)
			{
			    material.color = color;
			}
		}
	}
}
