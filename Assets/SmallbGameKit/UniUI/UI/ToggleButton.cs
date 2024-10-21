using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniUI
{
	[AddComponentMenu("UniUI/ToggleButton")]
	public class ToggleButton : MonoBehaviour
	{
		Toggle button;

		public Toggle Button
		{
			get
			{
				return button;
			}
		}

		public virtual void OnValueChange(bool value)
		{
		}

		protected virtual void OnAwake()
		{
		}

		void Awake()
		{
			button = GetComponent<Toggle>();
			if(Application.isPlaying)
				button.onValueChanged.AddListener(OnValueChange); 

			OnAwake();
		}

		void OnDestroy()
		{
			if(Application.isPlaying)
			{
				if(button != null)
					button.onValueChanged.RemoveListener(OnValueChange);
			}
		}
	}
}