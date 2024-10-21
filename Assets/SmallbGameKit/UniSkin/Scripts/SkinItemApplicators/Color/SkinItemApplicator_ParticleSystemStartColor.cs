using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_ParticleSystemStartColor")]
	public class SkinItemApplicator_ParticleSystemStartColor : SkinItemApplicator_ColorBase
	{
		ParticleSystem particleSystemComponent;

		protected override Color CurrentColor
		{
			get
			{
				if(particleSystemComponent == null)
				particleSystemComponent = GetComponent<ParticleSystem>();

				return particleSystemComponent.main.startColor.color;
			}
		}

		protected override void OnColorChange(Color color)
		{
			if(particleSystemComponent == null)
				particleSystemComponent = GetComponent<ParticleSystem>();

			ParticleSystem.MainModule settings = particleSystemComponent.main;
			settings.startColor = new ParticleSystem.MinMaxGradient(color);
		}
	}
}
