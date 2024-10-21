using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/ActivatorController_Continue")]
	public class ActivatorController_Continue : MonoBehaviour 
	{
		public UniActivation.Activator activator;

		void Awake()
		{
			ContinueManager.onContinueCountdownToggle += OnContinueCountdownToggle;

			OnContinueCountdownToggle(false);
		}

		void OnDestroy()
		{
			ContinueManager.onContinueCountdownToggle -= OnContinueCountdownToggle;
		}

		void OnContinueCountdownToggle(bool continueCountdownInProgress)
		{
			activator.SelectedIndex = continueCountdownInProgress?1:0;
		}
	}
}
