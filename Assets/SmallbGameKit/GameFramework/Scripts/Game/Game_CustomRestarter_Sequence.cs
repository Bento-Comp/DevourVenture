using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	[AddComponentMenu("GameFramework/Game_CustomRestarter_Sequence")]
	public class Game_CustomRestarter_Sequence : Game_CustomRestarter_Base
	{
		public List<Game_CustomRestarter_Base> restartersSequence;

		protected override void HandleCustomRestart()
		{
			int callCount = CallCount;

			if(callCount >= restartersSequence.Count)
			{
				DoRestart();
				return;
			}

			restartersSequence[callCount].AskForRestart();
		}
	}
}