using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	public interface IGameBehaviour 
	{
		void NotifyLoadGame();
		void NotifyLoadGameEnd(bool reloadSceneAfter);
		void NotifyGameStart();
		void NotifyInterlude();
		void NotifyInterludeEnd();
		void NotifyGameOver();
		void NotifyLevelCompleted(bool success);
	}
}