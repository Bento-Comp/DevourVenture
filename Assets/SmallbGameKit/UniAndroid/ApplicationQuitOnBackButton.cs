using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuitOnBackButton : MonoBehaviour
{
	#if UNITY_ANDROID
	void Update() 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("ApplicationQuitOnBackButton : Quit");
			#if !UNITY_EDITOR
			// Get the unity player activity
			AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			// call activity's boolean moveTaskToBack(boolean nonRoot) function
			// documentation: http://developer.android.com/reference/android/app/Activity.html#moveTaskToBack(boolean)
			activity.Call<bool>("moveTaskToBack", true);   //To suspend
			#endif
		}
	}
	#endif
}
