using System;
using UnityEngine;
using Firebase;

namespace JuicyInternal
{
    [DefaultExecutionOrder(-32000)]
    public class FirebaseManager : MonoBehaviour
    {
        static bool isInitialized = false;
        public static bool IsInitialized
        {
            get
            {
                return isInitialized;
            }
        }

        static Action<bool> onInitialisationComplete;
        bool doWaitOneFrameAfterInitilization = false;
        bool success = false;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (!doWaitOneFrameAfterInitilization || isInitialized)
                return;

            doWaitOneFrameAfterInitilization = false;
            OnInitializationComplete();
        }

        public static void AddOnInitializationComplete(Action<bool> onInitializationCompleteAction)
        {
            if (!isInitialized)
                onInitialisationComplete += onInitializationCompleteAction;
            else
                onInitializationCompleteAction?.Invoke(true);
        }

        void Initialize()
        {
            if (isInitialized)
            {
                JuicySDKLog.LogWarning("FirebaseManager : Initialize : FireBase is already initialized");
                return;
            }

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith
                (task =>
                    {
                        var dependencyStatus = task.Result;
                        if (dependencyStatus == Firebase.DependencyStatus.Available)
                        {
                            JuicySDKLog.Log("FirebaseManager : Initialize : Success");
                            success = true;
                        }
                        else
                        {
                            JuicySDKLog.LogError(String.Format("FirebaseManager : Initialize : Fail : Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                            success = false;
                        }
                        doWaitOneFrameAfterInitilization = true;
                    }
                );
        }

        void OnInitializationComplete()
        {
            JuicySDKLog.Log("FirebaseManager : Initialization complete : " + success);
            Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Error;
            Firebase.Crashlytics.Crashlytics.IsCrashlyticsCollectionEnabled = true;
            isInitialized = success;
            onInitialisationComplete?.Invoke(success);
        }
    }
}