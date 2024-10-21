using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Juicy;

namespace JuicyInternal
{
	[DefaultExecutionOrder(-31700)]
	[AddComponentMenu("JuicySDKInternal/JuicyRemoveAdsManager")]
	public class JuicyRemoveAdsManager : MonoBehaviour
	{		
		public static Action onRemoveAds;

		static JuicyRemoveAdsManager instance;
		
		static public JuicyRemoveAdsManager Instance
		{
			get
			{
				return instance;
			}
		}
		
		static public bool AdsRemoved
		{
			get
			{
                //The player prefs one is to assure continuity with pre 1.0 version of the SDK
				return AdsRemovedSave || JuicySDK.Settings.DebugForceRemoveAds ||
                        PlayerPrefs.GetInt("JuicyRemoveAdsManager_AdsRemoved", 0) == 1;
			}
		}

		static bool AdsRemovedSave
		{
			get
			{
                return JuicyPlayerPrefs.GetBool(JuicyPlayerPrefs.ADS_REMOVED);
			}

			set
			{
                JuicyPlayerPrefs.SetBool(JuicyPlayerPrefs.ADS_REMOVED, value);
			}
		}

		public void RemoveAds()
		{
			if(AdsRemoved)
				return;

			JuicySDKLog.Verbose("JuicyRemoveAdsManager : RemoveAds");

			JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("removeAds");

			AdsRemovedSave = true;

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
				JuicySDKLog.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
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