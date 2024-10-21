using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

using UniActivation;
using GameFramework.SimpleGame;
using Coffee.UIExtensions;

namespace GameFramework.SimpleGame
{
	[ExecuteInEditMode]
	[AddComponentMenu("GameFramework/SimpleGame/UIShinyEffectRandomiser")]
	public class UIShinyEffectRandomiser : MonoBehaviour
	{
		public bool autoFill = true;
		public List<UIShiny> uiShinyEffects;

		public float loopDelayMin = 1.0f;
		public float loopDelayMax = 3.0f;

		public float durationMin = 0.3f;
		public float durationMax = 0.6f;

		public float delayBetweenEffectsInDurationPercent = 0.5f;

		float currentPlayDuration;
		float remainingTimeBeforeNextPlay;

		void Start()
		{
			ScheduleNextPlay();
		}

		void Update()
		{
			remainingTimeBeforeNextPlay -= Time.deltaTime;
			if(remainingTimeBeforeNextPlay <= 0.0f)
			{
				ExecuteNextPlay();
				ScheduleNextPlay();
			}
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			if(autoFill)
				AutoFill();
		}

		void AutoFill()
		{
			uiShinyEffects = new List<UIShiny>(GetComponentsInChildren<UIShiny>(true));
		}
		#endif

		void ExecuteNextPlay()
		{
			currentPlayDuration = Random.Range(durationMin, durationMax);

			List<UIShiny> uiShinyToPlay = new List<UIShiny>();
			foreach(UIShiny uiShiny in uiShinyEffects)
			{
				//if(uiShiny.isActiveAndEnabled == false)
					//continue;

				uiShiny.duration = currentPlayDuration;
				uiShinyToPlay.Add(uiShiny);
			}
			StartCoroutine(ExecuteShinySequence(uiShinyToPlay));
		}

		IEnumerator ExecuteShinySequence(List<UIShiny> uiShinyToPlay)
		{
			float delayBetweenEffects = currentPlayDuration * delayBetweenEffectsInDurationPercent;
			foreach(UIShiny uiShiny in uiShinyToPlay)
			{
				uiShiny.Play();
				yield return new WaitForSeconds(delayBetweenEffects);
			}
		}

		void ScheduleNextPlay()
		{
			remainingTimeBeforeNextPlay = Random.Range(loopDelayMin, loopDelayMax);
		}
	}
}