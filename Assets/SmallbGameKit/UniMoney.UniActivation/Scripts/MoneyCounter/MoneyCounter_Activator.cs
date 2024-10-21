using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniActivation;

namespace UniMoney
{
	[AddComponentMenu("UniMoney/MoneyCounter_Activator")]
	public class MoneyCounter_Activator : MonoBehaviour
	{
		public Activator activator;

		public string moneyName;

		public int moneyValueByStep = 50;

		int MoneyValue => MoneyManager.Instance.GetMoney(moneyName);

		int CurrentStep
		{
			get
			{
				int moneyValue = MoneyValue;
				int step = moneyValue/moneyValueByStep;

				return step;
			}
		}

		void OnEnable()
		{
			activator.SetFirstActiveState(CurrentStep);
		}

		void Awake()
		{
			MoneyManager.onMoneyChange += OnMoneyChange;
		}

		void OnDestroy()
		{
			MoneyManager.onMoneyChange -= OnMoneyChange;
		}

		void OnMoneyChange(string moneyName)
		{
			if(this.moneyName != moneyName)
				return;

			activator.SelectedIndex = CurrentStep;
		}
	}
}
