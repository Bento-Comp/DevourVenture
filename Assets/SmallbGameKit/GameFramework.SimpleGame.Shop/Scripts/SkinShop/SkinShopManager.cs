using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UniSkin;
using UniMoney;
using UniHapticFeedback;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-32000)]
	[SelectionBase()]
	[AddComponentMenu("GameFramework/SimpleGame/SkinShopManager")]
	public class SkinShopManager : MonoBehaviour
	{
		[System.Serializable]
		public class UnlockClickInfos
		{
			public string moneyName = "Crown";
			public int clickValue = 1;
			public GameObject clickFxPrefab_hit;
			public GameObject clickFxPrefab_kill;
		}

		public Transform clickFxRoot;

		public List<UnlockClickInfos> unlockClickInfosList;

		public Dictionary<string, UnlockClickInfos> unlockClickValueByName = new Dictionary<string, UnlockClickInfos>();

		static SkinShopManager instance;

		public int GetUnlockClickValue(string unlockMoneyName)
		{
			UnlockClickInfos unlockClickInfos;
			if(unlockClickValueByName.TryGetValue(unlockMoneyName, out unlockClickInfos))
			{
				if(unlockClickInfos != null)
					return unlockClickInfos.clickValue;
			}

			return 0;
		}

		public GameObject SpawnUnlockClickFx_Kill(string unlockMoneyName, Vector3 position)
		{
			GameObject fxPrefab = GetUnlockClickFxPrefab_Kill(unlockMoneyName);

			GameObject fxInstance = Instantiate<GameObject>(fxPrefab, position, Quaternion.identity, clickFxRoot);

			return fxInstance;
		}

		public GameObject SpawnUnlockClickFx_Hit(string unlockMoneyName, Vector3 position)
		{
			GameObject fxPrefab = GetUnlockClickFxPrefab_Hit(unlockMoneyName);

			GameObject fxInstance = Instantiate<GameObject>(fxPrefab, position, Quaternion.identity, clickFxRoot);

			return fxInstance;
		}

		public static SkinShopManager Instance
		{
			get
			{
				return instance;
			}
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

			FillDictionary();
		}
		
		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		void FillDictionary()
		{
			foreach(UnlockClickInfos unlockClickInfos in unlockClickInfosList)
			{
				unlockClickValueByName[unlockClickInfos.moneyName] = unlockClickInfos;
			}
		}

		GameObject GetUnlockClickFxPrefab_Hit(string unlockMoneyName)
		{
			UnlockClickInfos unlockClickInfos;
			if(unlockClickValueByName.TryGetValue(unlockMoneyName, out unlockClickInfos))
			{
				if(unlockClickInfos != null)
					return unlockClickInfos.clickFxPrefab_hit;
			}

			return null;
		}

		GameObject GetUnlockClickFxPrefab_Kill(string unlockMoneyName)
		{
			UnlockClickInfos unlockClickInfos;
			if(unlockClickValueByName.TryGetValue(unlockMoneyName, out unlockClickInfos))
			{
				if(unlockClickInfos != null)
					return unlockClickInfos.clickFxPrefab_kill;
			}

			return null;
		}
	}
}