using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniApplication
{
	[AddComponentMenu("UniApplication/Render/gkRenderQueueOverride_Material")]
	public class RenderQueueOverride_Material : MonoBehaviour 
	{
		public int renderQueue = 3000;
		
		public Material material;
		
		public void Update()
		{
			if(material.renderQueue != renderQueue)
			{
				material.renderQueue = renderQueue;
			}
		}
	}
}