using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameFramework;
using UniUI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/MoneyRewardUI")]
	public class MoneyRewardUI : MonoBehaviour
	{
		public MoneyUIGiver moneyUIGiver;

		public MoneyRewardUI_Deactivator deactivator;
	}
}
