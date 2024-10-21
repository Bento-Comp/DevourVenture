#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UniEditor
{
public class PlayerPrefsMenuItems : Editor
	{
		[MenuItem("UniEditor/Delete All player prefs")]
		static void DoDeleteAllPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();
		}
	}
}
#endif