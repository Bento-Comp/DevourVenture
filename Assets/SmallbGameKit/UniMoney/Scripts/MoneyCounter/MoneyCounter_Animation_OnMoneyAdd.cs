using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniMoney
{
	[AddComponentMenu("UniMoney/MoneyCounter_Animation_OnMoneyAdd")]
	public class MoneyCounter_Animation_OnMoneyAdd : MonoBehaviour 
	{
		public string moneyName;

		public Animator animator;

		public string gainMoneyTriggerName = "Bump";

		void Awake()
		{
			MoneyManager.onMoneyAdd += OnMoneyAdd;
		}

		void OnDestroy()
		{
			MoneyManager.onMoneyAdd -= OnMoneyAdd;
		}

		void OnMoneyAdd(string moneyName, int addedValue)
		{
			if(this.moneyName != moneyName)
				return;

			animator.enabled = true;
			animator.SetTrigger(gainMoneyTriggerName);
		}
	}
}