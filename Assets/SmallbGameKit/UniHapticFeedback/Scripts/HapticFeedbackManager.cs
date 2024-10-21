using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniHapticFeedback
{
	public enum ForceHapticFeedbackSupportMode
	{
		DontForce,
		ForceSupported,
		ForceNotSupported
	}

	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("UniHapticFeedback/HapticFeedbackManager")]
	public class HapticFeedbackManager : UniSingleton.Singleton<HapticFeedbackManager>
	{
		[System.Serializable]
		public class HapticFeedbackDuration
		{
			public EHapticFeedbackType feedbackType;
			public float feedbackDuration = 0.1f;

			public HapticFeedbackDuration(EHapticFeedbackType feedbackType, float feedbackDuration)
			{
				this.feedbackType = feedbackType;
				this.feedbackDuration = feedbackDuration;
			}
		}

		public bool hapticFeedbackEnabled = true;

		public List<HapticFeedbackDuration> feedbackDurations = new List<HapticFeedbackDuration>()
		{
			new HapticFeedbackDuration(EHapticFeedbackType.SelectionChange, 0.05f),
			new HapticFeedbackDuration(EHapticFeedbackType.Light, 0.1f),
			new HapticFeedbackDuration(EHapticFeedbackType.Medium, 0.15f),
			new HapticFeedbackDuration(EHapticFeedbackType.Heavy, 0.2f),
			new HapticFeedbackDuration(EHapticFeedbackType.Success, 0.5f),
			new HapticFeedbackDuration(EHapticFeedbackType.Warning, 0.5f),
			new HapticFeedbackDuration(EHapticFeedbackType.Failure, 0.5f)
		};

		[Header("Limit Haptic Time Coverage")]
		public float maxHapticTimeCoverage = 0.5f;
		public float hapticTimeCoverageComputePeriod_Increase = 0.5f;
		public float hapticTimeCoverageComputePeriod_Decrease = 1.0f;

		public float debug_hapticTimeCoverage;
		public float debug_hapticTimeDilatation;

		public bool debug_logEnabled;

		public bool simulateHapticWithVibrationOnAndroid;

		public ForceHapticFeedbackSupportMode editor_forceHapticSupportMode = ForceHapticFeedbackSupportMode.ForceSupported;

		static string hapticFeedbackUserEnable_savekey = "HapticFeedbackUserEnable";

		bool hapticFeedbackSupported;

		float hapticTimeCoverage;

		// Feedback have duration and priority on each other to avoid heating too much the device
		Dictionary<EHapticFeedbackType, HapticFeedbackDuration> durationByFeedbackType = new Dictionary<EHapticFeedbackType, HapticFeedbackDuration>();
		bool feedbackInProgress;
		float feedbackRemainingTime;
		float undilatedFeedbackRemainingTime;
		EHapticFeedbackType activeFeedbackType = EHapticFeedbackType.None;

		bool userHapticFeedbackEnable;
		public bool UserHapticFeedbackEnable
		{
			get
			{
				return userHapticFeedbackEnable;
			}

			set
			{
				userHapticFeedbackEnable = value;
				_UserHapticFeedbackEnable_Save = value;
			}
		}

		public static bool HapticFeedbackEnabled
		{
			get
			{
				if(Instance == null)
					return false;

				return Instance.isActiveAndEnabled
					&&
					Instance.hapticFeedbackEnabled
					&&
					Instance.userHapticFeedbackEnable
					&&
					Instance.hapticFeedbackSupported;
			}

			set
			{
				if(Instance == null)
					return;

				Instance.hapticFeedbackEnabled = value;
			}
		}

		static bool Debug_LogEnabled
		{
			get
			{
				if(Instance == null)
					return false;

				return Instance.isActiveAndEnabled && Instance.debug_logEnabled;
			}
		}

		public bool HapticFeedbackSupported
		{
			get
			{
				return hapticFeedbackSupported;
			}
		}

		bool _HapticFeedbackSupported
		{
			get
			{
#if UNITY_EDITOR
				switch(editor_forceHapticSupportMode)
				{
					case ForceHapticFeedbackSupportMode.ForceSupported:
						return true;

					case ForceHapticFeedbackSupportMode.ForceNotSupported:
						return false;
				}
#endif

#if UNITY_ANDROID
				return simulateHapticWithVibrationOnAndroid;
#else
				//return LofeltHaptics.DeviceMeetsMinimumPlatformRequirements();
				//return MMVibrationManager.HapticsSupported();
				return iOSHapticFeedback.Instance != null && iOSHapticFeedback.Instance.IsSupported();
#endif
			}
		}

		static bool _UserHapticFeedbackEnable_Save
		{
			get
			{
				return PlayerPrefs.GetInt(hapticFeedbackUserEnable_savekey, 1) == 1;
			}

			set
			{
				PlayerPrefs.SetInt(hapticFeedbackUserEnable_savekey, value ? 1 : 0);
			}
		}

		public static void TriggerHapticFeedback(EHapticFeedbackType feedbackType, bool canIgnore = false)
		{
			if(HapticFeedbackEnabled == false)
				return;

			if(Debug_LogEnabled)
			{
				//Debug.Log("TriggerHapticFeedback : " + feedbackType);
			}

			Instance.TryPlayHapticFeedback(feedbackType, canIgnore);
		}

		void TryPlayHapticFeedback(EHapticFeedbackType feedbackType, bool canIgnore)
		{
			bool havePriority = HavePriority(feedbackType);

			if(canIgnore && havePriority == false)
				return;

			PlayHapticFeedback(feedbackType, havePriority);
		}

		bool HavePriority(EHapticFeedbackType feedbackType)
		{
			if(feedbackType == EHapticFeedbackType.None)
				return false;

			if(feedbackInProgress == false)
				return true;

			if(activeFeedbackType == EHapticFeedbackType.None)
				return true;

			return (int)feedbackType > (int)activeFeedbackType;
		}

		void PlayHapticFeedback(EHapticFeedbackType feedbackType, bool havePriority)
		{
			if(Debug_LogEnabled)
			{
				Debug.Log("PlayHapticFeedback : " + feedbackType + ", have priority = " + havePriority);
			}

			if(havePriority)
			{
				activeFeedbackType = feedbackType;
				feedbackInProgress = true;
				feedbackRemainingTime = durationByFeedbackType[feedbackType].feedbackDuration;
				undilatedFeedbackRemainingTime = feedbackRemainingTime;
			}

			switch(feedbackType)
			{
				case EHapticFeedbackType.SelectionChange:
					{
						//HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
						//MMVibrationManager.Haptic(HapticTypes.Selection);
						iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
					}
					break;

				case EHapticFeedbackType.Light:
					{
						//HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
						//MMVibrationManager.Haptic(HapticTypes.LightImpact);
						iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
					}
					break;

				case EHapticFeedbackType.Medium:
					{
						//HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
						//MMVibrationManager.Haptic(HapticTypes.MediumImpact);
						iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
					}
					break;

				case EHapticFeedbackType.Heavy:
					{
						//HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
						//MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
						iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);
					}
					break;

				case EHapticFeedbackType.Success:
					{
						//HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
						//MMVibrationManager.Haptic(HapticTypes.Success);
						iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
					}
					break;

				case EHapticFeedbackType.Warning:
					{
						//HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
						//MMVibrationManager.Haptic(HapticTypes.Warning);
						iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Warning);
					}
					break;
				case EHapticFeedbackType.Failure:
					{
						//HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
						//MMVibrationManager.Haptic(HapticTypes.Failure);
						iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);
					}
					break;

				case EHapticFeedbackType.None:
				default:
					{
					}
					break;
			}
		}

		void Awake()
		{
			userHapticFeedbackEnable = _UserHapticFeedbackEnable_Save;
			hapticFeedbackSupported = _HapticFeedbackSupported;

			FillDictionary();
		}

		void FillDictionary()
		{
			foreach(HapticFeedbackDuration duration in feedbackDurations)
			{
				durationByFeedbackType.Add(duration.feedbackType, duration);
			}
		}

        void LateUpdate()
        {
			UpdateUndilatedFeedbackRemainingTime();

			UpdateHapticTimeCoverage();

			UdpateFeedbackInProgress();
		}

		void UdpateFeedbackInProgress()
		{
			if(feedbackInProgress == false)
				return;

			float timeDilatation = ComputeTimeDilatationToEnsureMaxTimeCoverage();
			debug_hapticTimeDilatation = timeDilatation;

			feedbackRemainingTime -= Time.deltaTime * timeDilatation;
			if(feedbackRemainingTime <= 0.0f)
			{
				feedbackInProgress = false;
			}
		}

		void UpdateUndilatedFeedbackRemainingTime()
		{
			if(undilatedFeedbackRemainingTime > 0.0f)
			{
				undilatedFeedbackRemainingTime -= Time.deltaTime;

				if(undilatedFeedbackRemainingTime <= 0.0f)
					undilatedFeedbackRemainingTime = 0.0f;
			}
		}

		void UpdateHapticTimeCoverage()
		{
			if(undilatedFeedbackRemainingTime > 0.0f)
			{
				hapticTimeCoverage += Time.deltaTime/ hapticTimeCoverageComputePeriod_Increase;
			}
			else
			{
				hapticTimeCoverage -= Time.deltaTime/hapticTimeCoverageComputePeriod_Decrease;
			}
			hapticTimeCoverage = Mathf.Clamp01(hapticTimeCoverage);
			debug_hapticTimeCoverage = hapticTimeCoverage;
		}

		float ComputeTimeDilatationToEnsureMaxTimeCoverage()
		{
			float timeDilatation = maxHapticTimeCoverage / hapticTimeCoverage;

			if(timeDilatation < 1.0f)
				timeDilatation = 1.0f;

			return timeDilatation;
		}
	}
}