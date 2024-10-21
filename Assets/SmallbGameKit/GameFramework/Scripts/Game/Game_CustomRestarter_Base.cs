using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	[AddComponentMenu("GameFramework/Game_CustomRestarter_Base")]
	public abstract class Game_CustomRestarter_Base : MonoBehaviour
	{
		int callCount;

		protected int CallCount => callCount;

		public void AskForRestart()
		{
			OnAskForRestart();
			++callCount;
		}

		protected virtual void OnAskForRestart()
		{
			HandleCustomRestart();
		}

		protected abstract void HandleCustomRestart();

		protected void DoRestart()
		{
			Game.Instance.DoRestart();
		}
	}
}