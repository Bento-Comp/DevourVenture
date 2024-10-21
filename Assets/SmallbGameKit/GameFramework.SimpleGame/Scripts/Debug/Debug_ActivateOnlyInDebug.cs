using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/Debug_ActivateOnlyInDebug")]
	public class Debug_ActivateOnlyInDebug : MonoBehaviour 
	{
		bool Active
		{
			set
			{
				gameObject.SetActive(value);
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