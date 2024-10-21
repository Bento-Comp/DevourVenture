using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniGameMode
{
	[AddComponentMenu("UniGameMode/ActiveOnGameMode")]
	public class ActiveOnGameMode : MonoBehaviour
	{	
		public GameObject controlledGameObject;

		public List<string> gameModes;

		public bool active = true;

		bool Active
		{
			set
			{
				controlledGameObject.SetActive(value);
			}
		}

		bool MustBeActive
		{
			get
			{
				bool found = gameModes.Contains(GameModeManager.Instance.GameMode);

				if(found)
				{
					return active;
				}
				else
				{
					return !active;
				}
			}
		}

		void Awake()
		{
			if(controlledGameObject == null)
				controlledGameObject = gameObject;
			
			GameModeManager.onGameModeChange += OnGameModeChange;
			OnGameModeChange();
		}

		void OnDestroy()
		{
			GameModeManager.onGameModeChange -= OnGameModeChange;
		}

		void OnGameModeChange()
		{
			Active = MustBeActive;
		}
	}
}