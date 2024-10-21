using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Juicy;

namespace JuicyInternal
{
    public static class JuicySDKLog
	{
        static public void Verbose(string message)
		{
        if (!JuicySDKSettings.Instance.UseVerboseLogs)
                return;

#if UNITY_EDITOR
            if (JuicySDKSettings.Instance.ShowLogInEditor)
                Debug.Log(message);
#else
                    Debug.Log(message);
#endif
		}

        static public void Log(string message)
        {
#if UNITY_EDITOR
            if (JuicySDKSettings.Instance.ShowLogInEditor)
                    Debug.Log(message);
#else
            Debug.Log(message);
#endif
        }

        static public void LogWarning(string message)
        {
#if UNITY_EDITOR
            if (JuicySDKSettings.Instance.ShowLogInEditor)
                    Debug.LogWarning(message);
#else
            Debug.LogWarning(message);
#endif
        }

        static public void LogError(string message)
        {
#if UNITY_EDITOR
            if (JuicySDKSettings.Instance.ShowLogInEditor)
                    Debug.LogError(message);
#else
            Debug.LogError(message);
#endif
        }
    }
}