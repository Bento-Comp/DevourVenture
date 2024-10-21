using UnityEngine;

using UniActivation;

namespace UniUI.UniActivation
{
	[AddComponentMenu("UniUI/ActivatorRadioButton")]
	public class ActivatorRadioButton : MenuButton
	{
		public System.Action onSelect;

		public Activator radioButtonsActivator;

		public int radioIndex = 0;

		public void SetInitialRadioButton()
		{
			radioButtonsActivator.SetFirstActiveState(radioIndex);
		}

		public override void OnClick()
		{
			if(radioButtonsActivator.SelectedIndex != radioIndex)
			{
				radioButtonsActivator.SelectedIndex = radioIndex;
				OnSelect();
			}
		}

		void OnSelect()
		{
			onSelect?.Invoke();
		}
	}
}