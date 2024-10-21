using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	[DefaultExecutionOrder(32000)]
	[AddComponentMenu("GameFramework/CallFirstLoadGame_OnLastAwake")]
	public class CallFirstLoadGame_OnLastAwake : MonoBehaviour 
	{
		void Awake()
		{
			Game.Instance.CallFirstLoadGame();
		}
	}
}
