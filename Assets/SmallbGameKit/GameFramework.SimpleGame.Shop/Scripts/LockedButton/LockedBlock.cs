using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UniSkin;
using UniMoney;
using UniHapticFeedback;

namespace GameFramework.SimpleGame
{
	[SelectionBase()]
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/LockedBlock")]
	public class LockedBlock : MonoBehaviour
	{
		[SerializeField]
		int value;

		[SerializeField]
		int displayValueStepColorDivider = 1;

		public Text valueTextComponent;

		public Image colorImage;

		public LockActivator lockActivator;

		public SkinSelector skinSelector;

		string defaultUnlockMoneyName = "Coin";

		public int RemainingCost
		{
			get
			{
				return SkinCost - BallSpent;
			}
		}

		public int Value
		{
			get
			{
				return value;
			}

			set
			{
				if(this.value != value)
				{
					this.value = value;
					OnValueChange();
				}
			}
		}

		public int SkinCost
		{
			get
			{
				return skinSelector.GetSkinItem<SkinItem_Int>("SkinCost").GetInt() * SkinShopManager.Instance.GetUnlockClickValue(UnlockMoneyName);
			}
		}

		public string UnlockMoneyName
		{
			get
			{
				SkinItem_String skinItem = null;

				skinItem = skinSelector.GetSkinItem<SkinItem_String>("UnlockMoneyName");

				if(skinItem == null)
					return defaultUnlockMoneyName;

				return skinItem.GetString();
			}
		}

		static string ballSpent_keySave = "LockedBlock_BallSpent";
		string BallSpent_KeySave
		{
			get
			{
				return ballSpent_keySave  + "_" + skinSelector.GetSkinItem<SkinItem_String>("NameID").GetString();	
			}
		}

		int BallSpent
		{
			get
			{
				return PlayerPrefs.GetInt(BallSpent_KeySave, 0);
			}

			set
			{
				PlayerPrefs.SetInt(BallSpent_KeySave, value);
			}
		}

		public void Hit(Vector3 position)
		{
			string unlockMoneyName = UnlockMoneyName;

			int clickValue = SkinShopManager.Instance.GetUnlockClickValue(unlockMoneyName);

			int tryToUseValue = Mathf.Min(clickValue, value);
			if(tryToUseValue <= 1)
				tryToUseValue = 1;

			int usedValue;
			if(MoneyManager.Instance.TryUseMoney(unlockMoneyName, tryToUseValue, true, out usedValue))
			{
				BallSpent += usedValue;
				value -= usedValue;

				HitFx(position);

				if(value <= 0)
				{
					KillBlock();
					return;
				}

				OnValueChange();
				HapticFeedbackManager.TriggerHapticFeedback(EHapticFeedbackType.Light);
			}
			else
			{
				HapticFeedbackManager.TriggerHapticFeedback(EHapticFeedbackType.Failure);
			}
		}

		public void KillBlock()
		{
			KillFx();
			lockActivator.Unlock();
			skinSelector.SelectSkin();
			HapticFeedbackManager.TriggerHapticFeedback(EHapticFeedbackType.Heavy);
		}

		public void Initialize()
		{
			Value = RemainingCost;
			displayValueStepColorDivider = SkinShopManager.Instance.GetUnlockClickValue(UnlockMoneyName);
		}

		void OnEnable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			OnValueChange();
		}

		#if UNITY_EDITOR || debugSkin
		void Update()
		{
			if(BlockColorManager.Instance == null)
				return;
			
			if(SkinManager.Instance.debug_updateSkinInRuntime == false && Application.isPlaying)
				return;
			
			OnValueChange();
		}
		#endif

		void OnValueChange()
		{
			valueTextComponent.text = value.ToString();
			Color color = BlockColorManager.Instance.GetBlockColor(value/displayValueStepColorDivider);
			colorImage.color = color;
		}

		void HitFx(Vector3 position)
		{
			SkinShopManager.Instance.SpawnUnlockClickFx_Hit(UnlockMoneyName, position);
		}

		void KillFx()
		{
			SkinShopManager.Instance.SpawnUnlockClickFx_Kill(UnlockMoneyName, transform.position);
		}
	}
}