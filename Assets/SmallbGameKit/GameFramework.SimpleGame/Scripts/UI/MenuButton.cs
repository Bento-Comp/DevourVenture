using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MenuButton")]
	public class MenuButton : MonoBehaviour
	{
		Button button;

		bool awaken;

		public Button Button
		{
			get
			{
				return button;
			}
		}

		public virtual void OnClick()
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnAwakeEnd()
		{
		}

		void Awake()
		{
			if(awaken)
				return;

			awaken = true;
					
			button = GetComponent<Button>();
			button.onClick.AddListener(OnClick); 

			OnAwake();
		}

		void OnDestroy()
		{
			if(button != null)
				button.onClick.RemoveListener(OnClick);

			if(awaken)
			{
				OnAwakeEnd();
			}
		}
	}
}