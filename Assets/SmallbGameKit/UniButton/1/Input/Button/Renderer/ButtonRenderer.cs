using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Button/Renderer/ButtonRenderer")]
	public abstract class ButtonRenderer : MonoBehaviour
	{
		public Button button;
		
		// Call this after created a buton renderer at runtime
		public void AfterRuntimeCreation()
		{
			Initialize();
		}
		
		protected abstract void SetUp();
		
		protected abstract void SetDown();
		
		protected virtual void OnEnable()
		{
			if(button != null)
			{
				Initialize();
			}
		}
		
		protected virtual void OnDisable()
		{
			if(button != null)
			{
				Terminate();
			}
		}
		
		protected virtual void Initialize()
		{
			if(button.enabled)
			{
				if(button.Pressed)
				{
					OnDown();
				}
				else
				{
					OnUp();
				}
			}
			
			button.onUp += OnUp;
			button.onDown += OnDown;
		}
		
		void Terminate()
		{
			button.onUp += OnUp;
			button.onDown += OnDown;
		}
		
		void OnUp()
		{
			SetUp();
		}
		
		void OnDown()
		{
			SetDown();
		}
	}
}