using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniApplication
{
	[AddComponentMenu("UniApplication/Render/gkRenderQueueOverride")]
	public class RenderQueueOverride : MonoBehaviour 
	{
		public int renderQueue = 3000;
		
		public bool useSharedMaterial = true;
		
		public void Update()
		{
			if(useSharedMaterial)
			{
				if(GetComponent<Renderer>().sharedMaterial.renderQueue != renderQueue)
				{
					GetComponent<Renderer>().sharedMaterial.renderQueue = renderQueue;
				}
			}
			else
			{
				if(GetComponent<Renderer>().material.renderQueue != renderQueue)
				{
					GetComponent<Renderer>().material.renderQueue = renderQueue;
				}
			}
		}
	}
}