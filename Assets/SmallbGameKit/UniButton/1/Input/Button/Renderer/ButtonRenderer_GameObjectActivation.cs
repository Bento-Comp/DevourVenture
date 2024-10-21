using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_GameObjectActivation")]
	public class ButtonRenderer_GameObjectActivation : ButtonRenderer
	{	
		public GameObject[] activateOnUp;
		
		public GameObject[] activateOnDown;
		
		protected override void SetUp()
		{
			Activate(activateOnDown, false);
			Activate(activateOnUp, true);
		}
		
		protected override void SetDown()
		{
			Activate(activateOnUp, false);
			Activate(activateOnDown, true);
		}
		
		void Activate(GameObject[] a_rGameObjects, bool a_bActivate)
		{
			foreach(GameObject rGameObject in a_rGameObjects)
			{
				if(rGameObject != null)
				{
					rGameObject.SetActive(a_bActivate);
				}
			}
		}
	}
}