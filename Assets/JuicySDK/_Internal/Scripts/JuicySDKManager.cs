using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Juicy;
using System;

namespace JuicyInternal
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("JuicySDKInternal/JuicySDKManager")]
	public class JuicySDKManager : MonoBehaviour
	{
		public JuicyPrivacyManager privacyManager;

		public JuicyAnalyticsManager analyticsManager;

		public JuicyAdsManager adsManager;

		public JuicyABTestManager abTestManager;

		public JuicyPurchaseManager purchaseManager;

		public JuicyRemoveAdsManager removeAdsManager;

		public JuicyPurchaseRemoveAdsManager purchaseRemoveAdsManager;

		public JuicyRatingManager ratingManager;

        public FirebaseManager firebaseManager;

        public JuicyAdsEmulation adsEmulation;

		static JuicySDKManager instance;
		public static JuicySDKManager Instance { get { return instance; } }

		public Action OnJuicySDKInitialized;

		void Awake()
		{
			if(instance != null)
			{
				if(instance != this)
					Destroy(gameObject);
				
				return;
			}

			instance = this;
			Init();

        }

        void CreateJuicySDK()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(this);

			CreateABTest();
			CreatePrivacy();
			CreateAnalytics();
            CreateAds();
			CreatePurchase();
			CreateRemoveAds();
			CreatePurchaseRemoveAds();
			CreateRating();
			CreateFireBase();

			#if UNITY_EDITOR || noJuicyCompilation
            CreateAdsEmulator();
			#endif

			#if debugJuicySDK
            gameObject.AddComponent<JuicySDKDebug>();
			#endif
        }

		void Init()
        {
			JuicySDKSettings.Instance.LoadAppConfig();
			CreateJuicySDK();
			UnityThread.initUnityThread();
			JuicySDKLaunchLog.Log();
		}

		public void OnPrivacySet()
        {
			JuicyAdsManager.Instance.Initialize();
			JuicyAnalyticsManager.Instance.Initialize();
			OnJuicySDKInitialized?.Invoke();
		}

        void CreatePrivacy()
		{
			JuicyUtility.CreateSubManager<JuicyPrivacyManager>(transform, privacyManager);
		}

        void CreateAnalytics()
		{
			JuicyUtility.CreateSubManager<JuicyAnalyticsManager>(transform, analyticsManager);
		}

		void CreateAds()
		{
			JuicyUtility.CreateSubManager<JuicyAdsManager>(transform, adsManager);
		}

		void CreateABTest()
		{
			JuicyUtility.CreateSubManager<JuicyABTestManager>(transform, abTestManager);
		}

		void CreatePurchase()
		{
			JuicyUtility.CreateSubManager<JuicyPurchaseManager>(transform, purchaseManager);
		}

		void CreateRemoveAds()
		{
			JuicyUtility.CreateSubManager<JuicyRemoveAdsManager>(transform, removeAdsManager);
		}

		void CreatePurchaseRemoveAds()
		{
			JuicyUtility.CreateSubManager<JuicyPurchaseRemoveAdsManager>(transform, purchaseRemoveAdsManager);
		}

		void CreateRating()
		{
			JuicyUtility.CreateSubManager<JuicyRatingManager>(transform, ratingManager);
		}

		void CreateFireBase()
		{
			JuicyUtility.CreateSubManager<FirebaseManager>(transform, firebaseManager);
		}

		void CreateAdsEmulator()
        {
            if(adsEmulation != null)
                Instantiate(adsEmulation, transform);
        }
	}
}