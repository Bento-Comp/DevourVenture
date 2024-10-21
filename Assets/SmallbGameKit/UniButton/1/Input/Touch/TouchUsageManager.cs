using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchUsageManager")]
	public class TouchUsageManager : MonoBehaviour
	{
		MultiValueDictionary<int, TouchButtonController> usedTouches = new MultiValueDictionary<int, TouchButtonController>();
		
		MultiValueDictionary<int, TouchButtonController> usedExclusivelyTouches = new MultiValueDictionary<int, TouchButtonController>();
		
		static TouchUsageManager ms_oInstance;
		
		static public bool IsTouchUsedExclusively(int a_iFingerId)
		{
			if(ms_oInstance != null)
			{
				if(ms_oInstance.usedExclusivelyTouches.GetValues(a_iFingerId) != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		
		static public void StartUseTouch(int a_iFingerId, TouchButtonController a_rCallerComponent, bool a_bExclusively)
		{
			if(ms_oInstance != null)
			{
				ms_oInstance.usedTouches.Add(a_iFingerId, a_rCallerComponent);
				if(a_bExclusively)
				{
					ms_oInstance.usedExclusivelyTouches.Add(a_iFingerId, a_rCallerComponent);
				}
			}
		}
		
		static public void StopUseTouch(int a_iFingerId, TouchButtonController a_rCallerComponent)
		{
			if(ms_oInstance != null)
			{
				ms_oInstance.usedTouches.Remove(a_iFingerId, a_rCallerComponent);
				ms_oInstance.usedExclusivelyTouches.Remove(a_iFingerId, a_rCallerComponent);
			}
		}
	
		static public void CancelOtherUsedTouches(int a_iFingerId, TouchButtonController a_rCallerComponent, bool a_bOnlyThoseWithLesserPriority = true)
		{
			if(ms_oInstance != null)
			{
				HashSet<TouchButtonController> oTouchButtonControllers;
				if(ms_oInstance.usedTouches.TryGetValue(a_iFingerId, out oTouchButtonControllers))
				{
					// Copy the hash set because removal can occur
					TouchButtonController[] oTouchButtonControllersArray = new TouchButtonController[oTouchButtonControllers.Count];
					oTouchButtonControllers.CopyTo(oTouchButtonControllersArray);
					
					foreach(TouchButtonController rButtonController in oTouchButtonControllersArray)
					{
						if( a_rCallerComponent != rButtonController
						   && (a_bOnlyThoseWithLesserPriority == false || rButtonController.TouchSortingOrder < a_rCallerComponent.TouchSortingOrder))
						{
							rButtonController.CancelTouch();
						}
					}
				}
			}
		}
		
		void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
	}
}