using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MoneyRewardUI_Deactivator")]
	public class MoneyRewardUI_Deactivator : MonoBehaviour
	{
		public UniActivation.ActivationStep activationStep;

		public void Deactivate()
		{
			activationStep.Exit();
		}
	}
}
