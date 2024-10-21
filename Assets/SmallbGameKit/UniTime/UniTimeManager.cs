using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniTime
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("UniTime/UniTimeManager")]
	public class UniTimeManager : UniSingleton.Singleton<UniTimeManager>
	{
        float initialFixedDeltaTime;

        float currentTimeScale = 1.0f;

        bool paused;

        public bool Paused => paused;

        public float TimeScale
        {
            get => Time.timeScale;

            set
            {
                if(paused)
                    return;

                currentTimeScale = value;
                SetUnityTimeScales(currentTimeScale, currentTimeScale);
            }
        }

        public void Pause()
        {
            paused = true;
            SetUnityTimeScales(0.0f, 0.0f);
        }

        public void Resume()
        {
            paused = false;
            SetUnityTimeScales(currentTimeScale, currentTimeScale);
        }

        void Awake()
        {
            initialFixedDeltaTime = Time.fixedDeltaTime;
        }

        void OnDestroy()
        {
            if(paused)
                Resume();

            TimeScale = 1.0f;
        }

        void SetUnityTimeScales(float updateTimeScale, float fixedUpdateTimeScale)
        {
            Time.timeScale = updateTimeScale;
            Time.fixedDeltaTime = fixedUpdateTimeScale * initialFixedDeltaTime;
        }
    }
}