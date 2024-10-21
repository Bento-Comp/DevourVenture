using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniMoney
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("UniMoney/MoneyManager")]
	public class MoneyManager : MonoBehaviour
	{
		public enum MoneyOperationType
		{
			Set,
			Add,
			Use
		}

		[System.Serializable]
		public class StartMoneyValue
		{
			public string moneyName;
			public int moneyValue;
			public bool reinitializeEachAwake;
		}

		public struct MoneyOperation
		{
			public string name;
			public MoneyOperationType type;
			public int value;
		}
		
		public static System.Action<string> onMoneyChange;

		public static System.Action<string, int> onMoneyAdd;

		static MoneyManager instance;

		public List<StartMoneyValue> startMoneyValues;

		MoneyOperation lastMoneyOperation;

		[Header("Debug")]
		public bool debug_useDebugStartMoneyValues;

		public List<StartMoneyValue> debug_startMoneyValues;

		public static MoneyManager Instance
		{
			get
			{
				return instance;
			}
		}

		public MoneyOperation LastMoneyOperation
		{
			get
			{
				return lastMoneyOperation;
			}
		}

		static string money_saveKey = "MoneyManager_Money";

		static string GetMoneySaveKey(string moneyName)
		{
			if(moneyName == "")
			{
				return money_saveKey;
			}

			return money_saveKey + "_" + moneyName;
		}

		public int GetMoney(string moneyName)
		{
			return PlayerPrefs.GetInt(GetMoneySaveKey(moneyName), 0);
		}

		public void SetMoney(string moneyName, int value)
		{
			_SetMoney(moneyName, value);

			SaveMoneyOperation(moneyName, value, MoneyOperationType.Set);
		}

		public void AddMoney(string moneyName, int value)
		{
			_SetMoney(moneyName, GetMoney(moneyName) + value);

			SaveMoneyOperation(moneyName, value, MoneyOperationType.Add);

			onMoneyAdd?.Invoke(moneyName, value);
		}

		public bool TryUseMoney(string moneyName, int value, bool useAllIfNotEnough = false)
		{
			int usedValue;
			return TryUseMoney(moneyName, value, useAllIfNotEnough, out usedValue);
		}

		public bool TryUseMoney(string moneyName, int value, bool useAllIfNotEnough, out int usedValued)
		{
			int money = GetMoney(moneyName);

			if(money < value)
			{
				if(useAllIfNotEnough)
				{
					usedValued = money;
				}
				else
				{
					usedValued = 0;
				}
			}
			else
			{
				usedValued = value;
			}

			if(usedValued <= 0)
				return false;
			
			_SetMoney(moneyName, money - usedValued);

			SaveMoneyOperation(moneyName, usedValued, MoneyOperationType.Use);

			return true;
		}

		void _SetMoney(string moneyName, int value)
		{
			PlayerPrefs.SetInt(GetMoneySaveKey(moneyName), value);
			
			onMoneyChange?.Invoke(moneyName);
		}

		void SaveMoneyOperation(string moneyName, int value, MoneyOperationType type)
		{
			lastMoneyOperation = new MoneyOperation();
			lastMoneyOperation.name = moneyName;
			lastMoneyOperation.value = value;
			lastMoneyOperation.type = type;
		}

		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}

			if(debug_useDebugStartMoneyValues)
			{
				InitializeMoneys(debug_startMoneyValues);
			}
			else
			{
				InitializeMoneys(startMoneyValues);
			}
		}
		
		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		static string money_initialized_saveKey = "MoneyManager_Money_Initialized";

		static string GetMoneyInitializedSaveKey(string moneyName)
		{
			if(moneyName == "")
			{
				return money_initialized_saveKey;
			}

			return money_initialized_saveKey + "_" + moneyName;
		}

		bool GetMoneyHaveBeenIntialized(string moneyName)
		{
			return PlayerPrefs.GetInt(GetMoneyInitializedSaveKey(moneyName), 0) == 1;
		}

		void SetMoneyHaveBeenIntiliazed(string moneyName, bool value)
		{
			PlayerPrefs.SetInt(GetMoneyInitializedSaveKey(moneyName), value?1:0);
		}

		void InitializeMoneys(List<StartMoneyValue> startMoneyValues)
		{
			foreach(StartMoneyValue startMoneyValue in startMoneyValues)
			{
				if(GetMoneyHaveBeenIntialized(startMoneyValue.moneyName) == false
					|| startMoneyValue.reinitializeEachAwake)
				{
					SetMoney(startMoneyValue.moneyName, startMoneyValue.moneyValue);
					SetMoneyHaveBeenIntiliazed(startMoneyValue.moneyName, true);
				}
			}
		}
	}
}