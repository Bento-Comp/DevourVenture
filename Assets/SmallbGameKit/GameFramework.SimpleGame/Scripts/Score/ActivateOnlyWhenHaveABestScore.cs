using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/ActivateOnlyWhenHaveABestScore")]
	public class ActivateOnlyWhenHaveABestScore : MonoBehaviour 
	{
		public GameObject activationRoot;

		bool started;

		void OnEnable()
		{
			if(started == false)
				return;
			
			UpdateActivation();
		}

		void Start()
		{
			started = true;
			UpdateActivation();
		}

		void UpdateActivation()
		{
			activationRoot.SetActive(ScoreManager.Instance.BestScore > 0);
		}
	}
}