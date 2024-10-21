using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/DebugActivator")]
	public class DebugActivator : MonoBehaviour 
	{
        public UniActivation.Activator activator;

		bool Active
		{
			set
			{
                activator.SelectedIndex = value?1:0;
            }
		}
		
		void Awake()
		{
			DebugManager.onDebugChange += OnDebugChange;
			UpdateActivation();
		}

		void OnDestroy()
		{
			DebugManager.onDebugChange -= OnDebugChange;
		}

		void OnDebugChange()
		{
			UpdateActivation();
		}

		void UpdateActivation()
		{
			Active = DebugManager.DebugEnabled;
		}
	}
}