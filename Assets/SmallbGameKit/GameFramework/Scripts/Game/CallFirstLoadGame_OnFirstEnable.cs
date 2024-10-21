using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	[DefaultExecutionOrder(32000)]
	[AddComponentMenu("GameFramework/CallFirstLoadGame_OnFirstEnable")]
	public class CallFirstLoadGame_OnFirstEnable : MonoBehaviour 
	{
		bool firstEnable = true;
		void OnEnable()
		{
			if(firstEnable == false)
				return;

			firstEnable = false;
			
			Game.Instance.CallFirstLoadGame();
		}
	}
}
