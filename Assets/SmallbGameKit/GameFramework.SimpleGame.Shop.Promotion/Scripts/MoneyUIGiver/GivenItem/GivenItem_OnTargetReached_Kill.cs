using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame.MoneyUIGiverInternal
{
	[DefaultExecutionOrder(-1)]
	[AddComponentMenu("GameFramework/SimpleGame/GivenItem_OnTargetReached_Kill")]
	public class GivenItem_OnTargetReached_Kill : MonoBehaviour
	{
		public GivenItem givenItem;

		void OnEnable()
		{
			givenItem.goToTarget.onTargetReached += OnTargetReached;
		}

		void OnDisable()
		{
			givenItem.goToTarget.onTargetReached -= OnTargetReached;
		}

		void OnTargetReached()
		{
			givenItem.Kill();
		}
	}
}