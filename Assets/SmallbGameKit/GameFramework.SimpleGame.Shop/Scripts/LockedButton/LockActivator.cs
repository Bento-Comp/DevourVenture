using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UniSkin;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/LockActivator")]
	public class LockActivator : MonoBehaviour
	{
		public LockedBlock lockedBlock;

		public UniActivation.Activator activator;

		public SkinSelector skinSelector;

		static string unlocked_keySave = "LockActivator_Unlocked";
		string Unlocked_KeySave
		{
			get
			{
				return unlocked_keySave  + "_" + skinSelector.GetSkinItem<SkinItem_String>("NameID").GetString();	
			}
		}

		public bool Unlocked
		{
			get
			{
				#if unlockAllSkins
				return true;
				#else
				return PlayerPrefs.GetInt(Unlocked_KeySave, 0) == 1;
				#endif
			}

			set
			{
                PlayerPrefs.SetInt(Unlocked_KeySave, value?1:0);
			}
		}

		public void Unlock()
		{
			Unlocked = true;
			SetUnlockedState();
		}

		void OnEnable()
		{
			lockedBlock.Initialize();
			int blockValue = lockedBlock.Value;

			if(Unlocked)
			{
				SetUnlockedState();
			}
			else if(blockValue <= 0)
			{
				Unlock();
			}
			else
			{
				SetLockedState();
			}
		}

		void SetUnlockedState()
		{
			activator.SelectedIndex = 1;
		}

		void SetLockedState()
		{
			activator.SelectedIndex = 0;
		}
	}
}