using UnityEngine;
using System.Collections;

namespace UniApplication
{
	[AddComponentMenu("UniApplication/SetScreenResolution ")]
	public class SetScreenResolution : MonoBehaviour
	{
		public float percent = 0.75f;

		public int maxScreenHeight = 1920;

		public int minScreenHeight = 480;

		float oldScreenHeight;

		bool screenResolutionChanged;

		static SetScreenResolution instance;

		public static SetScreenResolution Instance
		{
			get
			{
				return instance;
			}
		}

		public static float DownScalingPercent
		{
			get
			{
				if(instance == null)
					return 1.0f;

				return instance.EffectiveScreenScaleDown;
			}
		}

		float Percent
		{
			get
			{
				return percent;
			}
		}

		public float EffectiveScreenScaleDown
		{
			get
			{
				if(screenResolutionChanged == false)
					return 1.0f;
				
				float newScreenHeight = (float)Screen.height;

				float effectiveScreenScaleDown = newScreenHeight/oldScreenHeight;

				return effectiveScreenScaleDown;
			}
		}

		void Awake()
		{
			if(instance == null)
			{
				instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}

			oldScreenHeight = (float)Screen.height;
			screenResolutionChanged = true;

			int newScreenHeight = Mathf.Clamp((int)(Screen.height * Percent), minScreenHeight, maxScreenHeight);
			int newScreenWidth = (int)(newScreenHeight * ((float)Screen.width/(float)Screen.height));

			Screen.SetResolution(newScreenWidth, newScreenHeight, Screen.fullScreen, 0);
		}

		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}
	}
}