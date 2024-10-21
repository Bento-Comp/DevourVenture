using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniMoney
{
	[AddComponentMenu("UniMoney/MoneyCounter_ValueText_Base")]
	public abstract class MoneyCounter_ValueText_Base : MonoBehaviour 
	{
		public System.Action onTextChange;

		public string moneyName;

		protected abstract void SetText(string text);

		void OnEnable()
		{
			OnMoneyChange(moneyName);
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
			
			SetText(MoneyManager.Instance.GetMoney(moneyName).ToString());
			onTextChange?.Invoke();
		}
	}
}