using System;
using UnityEngine;
using Juicy;

#if UNITY_IOS
using Juicy.IOS;
#endif
#if UNITY_ANDROID
using Juicy.Android;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JuicyInternal
{
	[DefaultExecutionOrder(-31600)]
	[AddComponentMenu("JuicySDKInternal/JuicyRatingManager")]
	public class JuicyRatingManager : MonoBehaviour
	{
		const int minimumGameCountBeforeRating = 5;
		const int delayAfterLaunchBeforeRatingInSeconds = 60;
		const int postponeCooldownBeforeRatingInSeconds = 86400;

		const int debug_minimumGameCountBeforeRating = 1;
		const int debug_delayAfterLaunchBeforeRatingInSeconds = 0;
		const int debug_postponeCooldownBeforeRatingInSeconds = 60;

		string popUpTitle = "Like the game?";
		string popUpMessage = "Please take a moment to rate us.";
		string rateButton = "Rate";
		string postponeButton = "Later";

		bool hasBeenRated
		{
			get { return JuicyPlayerPrefs.GetBool(JuicyPlayerPrefs.RATING_HASBEENRATED, false); }
			set { JuicyPlayerPrefs.SetBool(JuicyPlayerPrefs.RATING_HASBEENRATED, value); }
		}

		int previousShowTime
		{
			get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.RATING_PREVIOUSSHOW, -1); }
            set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.RATING_PREVIOUSSHOW, value); }
        }

		int opportunityCounter
		{
			get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.RATING_COUNTER, 0); }
			set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.RATING_COUNTER, value); }
		}

		static JuicyRatingManager instance;
		public static JuicyRatingManager Instance
		{
			get
			{
				return instance;
			}
		}

		int MinimumGameCountBeforeRating
		{
			get
			{
				if(JuicySDK.Settings.DebugRating)
					return debug_minimumGameCountBeforeRating;
				return minimumGameCountBeforeRating;
			}
		}

		int DelayAfterLaunchBeforeRatingInSeconds
		{
			get
			{
				if(JuicySDK.Settings.DebugRating)
					return debug_delayAfterLaunchBeforeRatingInSeconds;
				return delayAfterLaunchBeforeRatingInSeconds;
			}
		}

		int PostponeCooldownBeforeRatingInSeconds
		{
			get
			{
				if(JuicySDK.Settings.DebugRating)
					return debug_postponeCooldownBeforeRatingInSeconds;
				return postponeCooldownBeforeRatingInSeconds;
			}
		}

		static string StoreURL
        {
            get
            {
#if (UNITY_EDITOR || UNITY_IOS)
				return String.Format("https://itunes.apple.com/app/id{0}?action=write-review",
					UnityEngine.Networking.UnityWebRequest.EscapeURL(JuicySDKSettings.Instance.BaseConfig.AppStoreId));

#else
				return String.Format("https://play.google.com/store/apps/details?id={0}",
					UnityEngine.Networking.UnityWebRequest.EscapeURL(JuicySDKSettings.Instance.BaseConfig.AppBundleID));
#endif

			}
		}

		void Awake()
		{
			if (instance == null)
				instance = this;

			else
			{
				JuicySDKLog.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}

		void OnDestroy()
		{
			if (instance == this)
				instance = null;
		}

		public void NotifyShowRateBoxOpportunity(bool bestScoreBeaten, bool success, int level)
		{
			JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rating_opportunity");

			bool mustShowRateBox = false;
			if(level == -1)
				mustShowRateBox = bestScoreBeaten;
			
			else
				mustShowRateBox = success;

			JuicySDKLog.Verbose("JuicyRatingManager : TryToShowRateBox : mustShowRateBox = " + mustShowRateBox);

			if(mustShowRateBox)
				TryToShowRateBox();
		}

		void TryToShowRateBox()
		{
			if (hasBeenRated)
				return;

			JuicySDKLog.Verbose("JuicyRatingManager : TryToShowRateBox");
			JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rating_tryShow");
			opportunityCounter++;
			if(ConditionsAreMet())
				ShowRateBox();
		}

		bool ConditionsAreMet()
        {
			if (Application.internetReachability == NetworkReachability.NotReachable)
            {
				JuicySDKLog.Verbose("JuicyRatingManager : ConditionsAreMet : Internet Not reachable");
				return false;
			}

			if (Time.realtimeSinceStartup < DelayAfterLaunchBeforeRatingInSeconds)
            {
				JuicySDKLog.Verbose("JuicyRatingManager : ConditionsAreMet : Delay After Launch not passed");
				return false;
			}
	
			if (opportunityCounter < MinimumGameCountBeforeRating)
            {
				JuicySDKLog.Verbose("JuicyRatingManager : ConditionsAreMet : Minimum Game Count not reached");
				return false;
			}

			if(previousShowTime > 0)
            {
				if (GetTime() - previousShowTime < PostponeCooldownBeforeRatingInSeconds)
				{
					JuicySDKLog.Verbose("JuicyRatingManager : ConditionsAreMet : Post pone time not reached");
					return false;
				}
            }

			return true;
        }

		void ShowRateBox()
		{
			JuicySDKLog.Verbose("JuicyRatingManager : ShowRateBox");
			JuicyAnalyticsManager.Instance.TrackAnalyticsEvent("rating_show");
			previousShowTime = GetTime();

#if UNITY_EDITOR
			if (EditorUtility.DisplayDialog(popUpTitle, popUpMessage, rateButton, postponeButton))
			{
				hasBeenRated = true;
				Application.OpenURL(StoreURL);
			}
#elif UNITY_IOS
			JuicyIOSPlugin.ShowRatingPopUp();
#elif UNITY_ANDROID
			JuicyAndroidPlugin.ShowPopUp(popUpTitle, popUpMessage, rateButton, postponeButton, "JuicyRatingManager", "OnAndroidPopUpCallBack");
#endif
		}

#if UNITY_ANDROID
		void OnAndroidPopUpCallBack(string message)
        {
			PopUpResponse response = JuicyAndroidPlugin.GetPopUpResponse(message);
            switch (response)
            {
				case PopUpResponse.Undefined:
					JuicySDKLog.LogError("JuicyRatingManager : OnAndroidPopUpCallBack : Undefined pop up response");
					break;
                case PopUpResponse.Positive:
					hasBeenRated = true;
					Application.OpenURL(StoreURL);
					break;
                case PopUpResponse.Negative:
				case PopUpResponse.Dismiss:
					break;
			}
        }
#endif

        int GetTime()
		{
			var epochStart = new System.DateTime(1980, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
			return (int)Math.Floor((System.DateTime.UtcNow - epochStart).TotalSeconds);
		}
    }
}
