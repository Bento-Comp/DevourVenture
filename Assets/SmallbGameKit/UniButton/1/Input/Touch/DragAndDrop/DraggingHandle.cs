using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/DragAndDrop/DraggingHandle")]
	public class DraggingHandle : MonoBehaviour
	{
		public Action<DraggingHandle> onStartDragging;
		
		public Action<DraggingHandle> onStopDragging;
		
		public Button button;
		
		public TouchButtonController touchButtonController;
		
		bool dragging;
		
		public bool IsDragging
		{
			get
			{
				return dragging;
			}
		}
		
		public float DraggingDistance
		{
			get
			{
				return touchButtonController.TouchDistanceFromStart_Pixel;
			}
		}
		
		public Vector2 DraggingMovement
		{
			get
			{
				return touchButtonController.TouchMovementFromStart_Pixel;
			}
		}
		
		public Vector2 DraggingPosition
		{
			get
			{
				return touchButtonController.CurrentTouchPosition;
			}
		}
		
		public Vector2 DraggingStartPosition
		{
			get
			{
				return touchButtonController.StartTouchPosition;
			}
		}
		
		public void ForceDraggingStart()
		{
			if(dragging == false)
			{
				float fSwipeMinDistanceSave = touchButtonController.swipe.minDistance_centimeter;
				touchButtonController.swipe.minDistance_centimeter  = 0;
				
				touchButtonController.ForceUpdateController();
				
				touchButtonController.swipe.minDistance_centimeter  = fSwipeMinDistanceSave;
			}
		}
		
		public void GiveToTouch(int a_iTouchIndex)
		{
			touchButtonController.ForceStartTouch(a_iTouchIndex, true);
		}
		
		void Start()
		{
			touchButtonController.onSwipe += OnSwipe;
			button.onUp += OnUp;
			if(touchButtonController.IsSwiping)
			{
				OnSwipe(true);
			}
		}
		
		void OnDestroy()
		{
			touchButtonController.onSwipe -= OnSwipe;
			button.onUp -= OnUp;
		}
		
		void OnSwipe(bool a_bSwiping)
		{
			if(a_bSwiping)
			{
				if(dragging == false)
				{
					StartDragging();
				}
			}
		}
		
		void OnUp()
		{
			if(dragging)
			{
				StopDragging();
			}
		}
		
		void StartDragging()
		{
			dragging = true;
			if(onStartDragging != null)
			{
				onStartDragging(this);
			}
		}
		
		void StopDragging()
		{
			dragging = false;
			if(onStopDragging != null)
			{
				onStopDragging(this);
			}
		}
	}
}