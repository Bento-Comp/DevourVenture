using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/RefuseMoneyRewardButtonActivator")]
	public class RefuseMoneyRewardButtonActivator : MonoBehaviour
	{
		public UniActivation.Activator activator;

		void Awake()
		{
			activator.SelectedIndex = 0;
		}

		void Start()
		{
			StartCoroutine(ActivationSequence());
		}

		IEnumerator ActivationSequence()
		{
			yield return new WaitForSecondsRealtime(MoneyRewardManager.Instance.canRefuseRewardDelay);
			activator.SelectedIndex = 1;
		}
	}
}