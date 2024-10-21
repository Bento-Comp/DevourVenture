using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UniSkin
{
	[AddComponentMenu("UniSkin/SkinItemApplicator_CameraBackgroundColor")]
	public class SkinItemApplicator_CameraBackgroundColor : SkinItemApplicator_ColorBase
	{
		Camera cameraComponent;

		protected override Color CurrentColor
		{
			get
			{
				if(cameraComponent == null)
				cameraComponent = GetComponent<Camera>();

				return cameraComponent.backgroundColor;
			}
		}

		protected override void OnColorChange(Color color)
		{
			if(cameraComponent == null)
				cameraComponent = GetComponent<Camera>();
			
			cameraComponent.backgroundColor = color;
		}
	}
}
