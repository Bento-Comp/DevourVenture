using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer_Color")]
	public abstract class ButtonRenderer_Color : ButtonRenderer
	{	
		public Color colorUp = Color.white;
		
		public Color colorDown = Color.black;
		
		public bool dontControlAlpha;

		public float fadeDuration = 0.1f;

		float fadePercent = 0.0f;

		float fadeDirection = 1.0f;

		Coroutine fadingCoroutine;

		protected abstract void SetColor(Color color);

		protected abstract Color GetColor();
		
		protected override void SetUp()
		{
			fadeDirection = -1.0f;

			StartFadingCoroutine();
		}
		
		protected override void SetDown()
		{
			fadeDirection = 1.0f;

			StartFadingCoroutine();
		}

		void StartFadingCoroutine()
		{
			if(fadingCoroutine == null)
			{
				fadingCoroutine = StartCoroutine(Fading());
			}
		}

		IEnumerator Fading()
		{
			while(true)
			{
				if(fadeDuration <= 0.0f)
				{
					FadeEnd();
					break;
				}

				fadePercent += fadeDirection * Time.deltaTime/fadeDuration;

				fadePercent = Mathf.Clamp01(fadePercent);

				if(
					(fadeDirection >= 0.0f && fadePercent >= 1.0f)
					||
					(fadeDirection < 0.0f && fadePercent <= 0.0f)
				)
				{
					FadeEnd();
					break;
				}

				SetFade(fadePercent);

				yield return null;
			}

			fadingCoroutine = null;
		}

		void FadeEnd()
		{
			if(fadeDirection >= 0.0f)
			{
				_SetColor(colorDown);
			}
			else
			{
				_SetColor(colorUp);
			}
		}

		void SetFade(float fade)
		{
			Color fadeColor = Color.Lerp(colorUp, colorDown, fade);
			_SetColor(fadeColor);
		}

		void _SetColor(Color color)
		{
			Color inputColor = GetColor();

			if(dontControlAlpha)
			{
				color.a = inputColor.a;
			}
			
			SetColor(color);
		}
	}
}