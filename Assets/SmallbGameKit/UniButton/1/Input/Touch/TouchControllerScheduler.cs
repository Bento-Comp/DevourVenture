using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchControllerScheduler")]
	public class TouchControllerScheduler : MonoBehaviour
	{
		List<TouchButtonController> touchControllersSortedByPriorities = new List<TouchButtonController>();
		
		static TouchControllerScheduler ms_oInstance;
		
		bool isSortingDirty;
		
		static public bool TryToRegister(TouchButtonController a_rTouchController)
		{
			if(ms_oInstance == null || a_rTouchController == null)
			{
				return false;	
			}
			else
			{
				ms_oInstance.Register(a_rTouchController);
				return true;
			}
		}
		
		static public bool TryToUnregister(TouchButtonController a_rTouchController)
		{
			if(ms_oInstance == null || a_rTouchController == null)
			{
				return false;	
			}
			else
			{
				ms_oInstance.Unregister(a_rTouchController);
				return true;
			}
		}
		
		static public void OnUpdateSorting(TouchButtonController a_rTouchController)
		{
			if(ms_oInstance != null && a_rTouchController != null)
			{
				ms_oInstance._OnUpdateSorting(a_rTouchController);
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
		
		void Update()
		{
			if(isSortingDirty)
			{
				SortTouchControllers();
			}
			UpdateControllers();
		}
		
		void Register(TouchButtonController a_rTouchController)
		{
			touchControllersSortedByPriorities.Add(a_rTouchController);
			SetSortingDirty();
		}
		
		void Unregister(TouchButtonController a_rTouchController)
		{
			touchControllersSortedByPriorities.Remove(a_rTouchController);
		}
		
		void SortTouchControllers()
		{
			touchControllersSortedByPriorities.Sort(CompareTouchController);
			isSortingDirty = false;
		}
		
		void _OnUpdateSorting(TouchButtonController a_rTouchController)
		{
			SetSortingDirty();
		}
		
		void SetSortingDirty()
		{
			// MAYBE_TODO_SEV :
			// If we ever want to optimize the sorting update, we can sort just the controllers of the dirty layer
			// instead of updating the sorting of all the controllers
			isSortingDirty = true;
		}
		
		void UpdateControllers()
		{
			foreach(TouchButtonController rTouchController in touchControllersSortedByPriorities)
			{
				if(rTouchController.gameObject.activeInHierarchy)
				{	
					rTouchController.SecondUpdateController();
				}
			}

			foreach(TouchButtonController rTouchController in touchControllersSortedByPriorities)
			{
				if(rTouchController.gameObject.activeInHierarchy)
				{	
					rTouchController.FirstUpdateController();
				}
			}
		}
		
		static int CompareTouchController(TouchButtonController a_rTouchControllerA, TouchButtonController a_rTouchControllerB)
		{
			int iCompareTouchSortingLayerID =  -(a_rTouchControllerA.TouchSortingLayerID).CompareTo(a_rTouchControllerB.TouchSortingLayerID);
			if(iCompareTouchSortingLayerID == 0)
			{
				return -(a_rTouchControllerA.TouchSortingOrder).CompareTo(a_rTouchControllerB.TouchSortingOrder);
			}
			else
			{
				return iCompareTouchSortingLayerID;
			}
		}
	}
}