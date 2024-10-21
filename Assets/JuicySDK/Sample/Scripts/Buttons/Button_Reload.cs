using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using Juicy;

namespace JuicySDKSample
{
	[AddComponentMenu("JuicySDKSample/Button_Reload")]
	public class Button_Reload : ButtonBase
	{
		protected override void OnClick()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}