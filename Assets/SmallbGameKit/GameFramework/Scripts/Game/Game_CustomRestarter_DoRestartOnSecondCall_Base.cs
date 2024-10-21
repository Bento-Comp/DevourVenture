using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	[AddComponentMenu("GameFramework/Game_CustomRestarter_DoRestartOnSecondCall_Base")]
	public abstract class Game_CustomRestarter_DoRestartOnSecondCall_Base : Game_CustomRestarter_Base
	{
		protected override void OnAskForRestart()
		{
			if(CallCount >= 1)
			{
				DoRestart();
				return;
			}

			HandleCustomRestart();
		}
	}
}