using UnityEngine;
using System.Collections;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchZone/TouchZoneBase")]
	public abstract class TouchZoneBase : MonoBehaviour
	{
		public Camera cameraComponent;
		
		public Camera GetCamera()
		{
			if(cameraComponent == null)
			{
				return Camera.main;
			}
			else
			{
				return cameraComponent;
			}
		}
		
		public bool ContainsScreenPoint(Vector2 a_f2ScreenPoint)
		{
			Camera rCamera = GetCamera();
			if(rCamera == null)
			{
				return false;
			}
			else
			{
				return _ContainsScreenPoint(a_f2ScreenPoint, rCamera);
			}
		}
		
		protected abstract bool _ContainsScreenPoint(Vector2 a_f2ScreenPoint, Camera a_rCamera);
		
		protected virtual void DisplayGizmos(Camera a_rCamera){}
	
		void OnDrawGizmos()
		{
			Camera rCamera = GetCamera();
			if(rCamera != null)
			{
				DisplayGizmos(rCamera);
			}
		}
	}
}