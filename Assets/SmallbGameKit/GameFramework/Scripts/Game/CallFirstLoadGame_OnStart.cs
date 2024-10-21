using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	[DefaultExecutionOrder(32000)]
	[AddComponentMenu("GameFramework/CallFirstLoadGame_OnStart")]
	public class CallFirstLoadGame_OnStart : MonoBehaviour 
	{
		void Start()
		{
			Game.Instance.CallFirstLoadGame();
		}
	}
}
