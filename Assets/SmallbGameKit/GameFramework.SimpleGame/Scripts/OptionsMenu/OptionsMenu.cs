using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UniActivation;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/OptionsMenu")]
	public class OptionsMenu : MonoBehaviour
	{
		public Activator activator;

		bool open;

		public void ToggleOpen()
		{
			Open(!open);
		}

		public void Open()
		{
			Open(true);
		}

		public void Close()
		{
			Open(false);
		}

		public void Open(bool open)
		{
			if(this.open == open)
				return;
			
			SetOpen(open);
		}

		void OnEnable()
		{
			SetOpen(false);
		}

		void OnDisable()
		{
			Close();
		}

		void SetOpen(bool open)
		{
			this.open = open;
			if(this.open)
			{
				activator.SelectedIndex = 1;
			}
			else
			{
				activator.SelectedIndex = 0;
			}
		}
	}
}