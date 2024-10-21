using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/RefuseSkinRewardButtonActivator")]
	public class RefuseSkinRewardButtonActivator : MonoBehaviour
	{
		public UniActivation.Activator activator;

		void OnEnable()
		{
			activator.SetFirstActiveState(0);
			StartCoroutine(ActivationSequence());
		}

		void OnDisable()
		{
			StopAllCoroutines();
		}

		IEnumerator ActivationSequence()
		{
			yield return new WaitForSecondsRealtime(SkinRewardManager.Instance.canRefuseRewardDelay);
			activator.SelectedIndex = 1;
		}
	}
}