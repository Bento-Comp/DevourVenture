using UnityEngine;

namespace UniUI.UniActivation
{
	[AddComponentMenu("UniUI/ActivatorRadioButton")]
	public class RadioButtonBase : MonoBehaviour
	{
		ActivatorRadioButton radioButton;

		bool awaken;

		public virtual void OnSelect()
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnAwakeEnd()
		{
		}

		protected void SetInitialRadioButton()
		{
			radioButton.SetInitialRadioButton();
		}

		void Awake()
		{
			if(awaken)
				return;

			awaken = true;

			radioButton = GetComponent<ActivatorRadioButton>();

			radioButton.onSelect += OnSelect;

			OnAwake();
		}

		void OnDestroy()
		{
			if(radioButton != null)
				radioButton.onSelect -= OnSelect;

			if(awaken)
			{
				OnAwakeEnd();
			}
		}
	}
}