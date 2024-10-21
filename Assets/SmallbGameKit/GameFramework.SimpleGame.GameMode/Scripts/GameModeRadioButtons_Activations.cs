using UnityEngine;

using UniUI.UniActivation;

using System.Collections.Generic;

namespace GameFramework.SimpleGame.GameMode
{
	[ExecuteAlways()]
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/GameModeRadioButtons_Activations")]
	public class GameModeRadioButtons_Activations : MonoBehaviour
	{
		public GameModeRadioButtons radioButtons;

		public bool deactiveAllWhenOnlyOneButtonActive = true;

		[SerializeField]
		List<bool> activateButtons = new List<bool>();

		public void SetActivations(List<bool> activations)
		{
			bool activationsChanged = false;

			if(activations.Count != activateButtons.Count)
			{
				activateButtons = new List<bool>(activations);
				activationsChanged = true;
			}
			else
			{
				for(int i = 0; i < activations.Count; ++i)
				{
					bool oldActivate = activateButtons[i];
					bool newActivate = activations[i];

					if(oldActivate == newActivate)
						continue;

					activateButtons[i] = newActivate;

					activationsChanged = true;
				}
			}

			if(activationsChanged)
				UpdateActivations();
		}

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			Debug_UpdateActivateButtonsList();
			UpdateActivations();
		}

		void Debug_UpdateActivateButtonsList()
		{
			int debugCount = activateButtons.Count;

			int buttonCount = radioButtons.buttons.Count;

			if(buttonCount > debugCount)
			{
				for(int i = debugCount; i < buttonCount; ++i)
				{
					activateButtons.Add(true);
				}
			}
			else if(buttonCount < debugCount)
			{
				for(int i = debugCount; i >= buttonCount; --i)
				{
					activateButtons.RemoveAt(i);
				}
			}
		}
#endif

		void UpdateActivations()
		{
			if(deactiveAllWhenOnlyOneButtonActive && CountButtonToActivate() <= 1)
			{
				foreach(GameModeRadioButton button in radioButtons.buttons)
				{
					SetActive(button, false);
				}
				return;
			}

			for(int i = 0; i < activateButtons.Count; ++i)
			{
				SetActive(radioButtons.buttons[i], activateButtons[i]);
			}
		}

		void SetActive(GameModeRadioButton button, bool active)
		{
			button.gameObject.SetActive(active);
		}

		int CountButtonToActivate()
		{
			int countButtonToActivate = 0;
			foreach(bool activateButton in activateButtons)
			{
				if(activateButton)
					++countButtonToActivate;
			}

			return countButtonToActivate;
		}
	}
}