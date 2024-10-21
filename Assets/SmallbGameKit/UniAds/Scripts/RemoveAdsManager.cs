using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniAds
{
	[AddComponentMenu("UniAds/RemoveAdsManager")]
	public class RemoveAdsManager : MonoBehaviour
	{		
		public static Action onRemoveAds;
		
		private bool adsRemoved;

		static string adsRemoved_saveKey = "AdsRemoved";

		static RemoveAdsManager instance;

		#if removeAds
		static bool debug_ForceRemoveAds = true;
		#else
		static bool debug_ForceRemoveAds = false;
		#endif
		
		static public RemoveAdsManager Instance
		{
			get
			{
				return instance;
			}
		}
		
		public bool AdsRemoved
		{
			get
			{
				return adsRemoved || debug_ForceRemoveAds;
			}
		}

		bool AdsRemovedSave
		{
			get
			{
				return (PlayerPrefs.GetInt(adsRemoved_saveKey, 0) == 1);
			}

			set
			{
				PlayerPrefs.SetInt(adsRemoved_saveKey, value?1:0);
			}
		}

		public void RemoveAds()
		{
			if(AdsRemoved)
				return;

			adsRemoved = true;
			AdsRemovedSave = adsRemoved;
			OnRemoveAds();
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
			
			InitializeAdsRemoved();
		}
		
		void InitializeAdsRemoved()
		{
			adsRemoved = AdsRemovedSave;
			if(AdsRemoved)
			{
				OnRemoveAds();
			}
		}
		
		void OnRemoveAds()
		{
			if(onRemoveAds != null)
			{
				onRemoveAds();
			}
		}
	}
}