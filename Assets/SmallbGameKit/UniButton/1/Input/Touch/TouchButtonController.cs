 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchButtonController")]
	public class TouchButtonController : MonoBehaviour
	{
		public enum ETouchStartMode
		{
			MustTap,
			DontNeedToTap,
			MustTapButDontNeedToOnEnable,
			MustTapButDontNeedToOnStart
		}
		
		public enum ETouchUsageMode
		{
			UseTouchExclusively,
			DontUseTouchExclusively
		}

		public enum ETouchEndMode
		{
			EndOnRelease,
			EndOnSwipe,
			EndOnSwipeOrOnRelease,
			EndOnHold
		}
		
		public enum ETouchCancelationMode
		{
			CancelWhenReleaseOutOfZone,
			CancelWhenCursorOutOfZone,
			CancelWhenSwiped,
			NeverCancel,
			CancelWhenHold,
			CancelWhenHoldAndSwipe
		}

		public enum EOtherTouchCancelMode
		{
			DontCancel,
			CancelAllOtherTouchesOnRelease,
			CancelLesserPriorityTouchOnRelease
		}
		
		public enum ETouchTransfertMode
		{
			SelectAnOtherValidTouchOnTouchEnd,
			DontTransfertTouch
		}
		
		enum ETouchEndType
		{
			Release,
			Swipe,
			Hold
		}
		
		enum ETouchInstigator
		{
			Mouse,
			Finger
		}
		
		[Serializable]
		public class ClickCountRules
		{
			public int numberOfClickNeeded = 1;
		
			public float clickRemanenceDuration = 0.1f;
		}
		
		[Serializable]
		public class ClickFilter
		{
			public List<int> mouseButtons = new List<int>{0};
		}
		
		[Serializable]
		public class SwipeParameters
		{
			public float minDistance_centimeter = 3.0f;
			
			public bool IsASwipe(Vector2 a_f2StartPosition, Vector2 a_f2CurrentPosition)
			{
				
				float fSwipeSquareDistance = ((a_f2CurrentPosition - a_f2StartPosition) * TouchUtility.PixelToCentimeter).sqrMagnitude;
				float fSwipeMinSquareDistance = (minDistance_centimeter * minDistance_centimeter);
				return fSwipeSquareDistance >= fSwipeMinSquareDistance;
			}
		}
		
		[Serializable]
		public class HoldParameters
		{
			public float minimumHoldDuration = 0.5f;
			
			float holdDuration;
			
			public void StartTouch()
			{
				holdDuration = 0.0f;
			}
			
			public void UpdateTouch()
			{
				holdDuration += Time.deltaTime;
			}
			
			public bool HasHeldLongEnough()
			{
				return holdDuration >= minimumHoldDuration;
			}
		}
		
		class TouchInfos
		{
			ETouchInstigator touchInstigator;
			
			int touchIndex = -1;
			
			Vector2 touchPosition;
			
			Vector2 startTouchPosition;
			
			public Vector2 StartTouchPosition
			{
				get
				{
					return startTouchPosition;
				}
				
				set
				{
					startTouchPosition = value;
				}
			}
			
			public Vector2 CurrentTouchPosition
			{
				get
				{
					return touchPosition;
				}
			}
			
			public int TouchIndex
			{
				get
				{
					return touchIndex;
				}
				
				set
				{
					touchIndex = value;
				}
			}
			
			public void StartTouch(ETouchInstigator a_eTouchInstigator, int a_iMouseButtonIndex, Vector2 a_f2TouchPosition)
			{
				touchInstigator = a_eTouchInstigator;
				touchIndex = a_iMouseButtonIndex;
				startTouchPosition = a_f2TouchPosition;
				touchPosition = a_f2TouchPosition;
			}
			
			public bool TryToUpdateTouchPosition()
			{
				switch(touchInstigator)
				{
					case ETouchInstigator.Finger:
					{
						Touch rCurrentTouch;
					
						// Get the current touch
						if(TouchUtility.TryGetTouchByFingerId(touchIndex, out rCurrentTouch))
						{
							if((rCurrentTouch.phase != TouchPhase.Ended && rCurrentTouch.phase != TouchPhase.Canceled))
							{
								touchPosition = rCurrentTouch.position;
								return true;
							}
						}
					}
					break;
					
					case ETouchInstigator.Mouse:
					{
						if(Input.GetMouseButton(touchIndex))
						{
							touchPosition = Input.mousePosition;
							return true;
						}
					}
					break;
				}
				
				return false;
			}
		}
		
		public Action<bool> onSwipe;
		
		public Action afterUpdate;
		
		[SerializeField]
		string touchSortingLayerName = "";
		
		[SerializeField]
		int touchSortingOrder = 0;
		
		public Button controlledButton;
		
		public ETouchStartMode touchStartMode;
		
		public ETouchUsageMode touchUsageMode;
		
		public ETouchEndMode touchEndMode;
		
		public ETouchCancelationMode touchCancelationMode;

		public EOtherTouchCancelMode otherTouchCancelMode;
		
		public ETouchTransfertMode touchTransfertMode;
		
		public SwipeParameters swipe = new SwipeParameters();
		
		public HoldParameters hold = new HoldParameters();
			
		public ClickCountRules clickCountRules = new ClickCountRules();
		
		public ClickFilter clickFilter = new ClickFilter();
		
		// the touch zones
		public List<TouchZoneBase> touchZones = new List<TouchZoneBase>();
		
		public List<TouchZoneBase> touchZonesExcluded = new List<TouchZoneBase>();
		
		bool touched;
		
		TouchInfos currentTouchInfos = new TouchInfos();
		
		int numberOfClick;
		
		float timeSinceLastClick;
		
		bool touchHasBeenCanceled;
		
		ETouchEndType touchEndType;
		
		bool buttonEnabled;
		
		bool useCurrentTouch;
		
		bool manageByScheduler;
		
		bool swiping;
		
		int touchSortingLayerID;
		
		public string TouchSortingLayerName
		{
			get
			{
				return touchSortingLayerName;
			}
			
			set
			{
				if(value != touchSortingLayerName)
				{
					touchSortingLayerName = value;
					
					AccordTouchLayerIDToName();
					
					TouchControllerScheduler.OnUpdateSorting(this);
				}
			}
		}
		
		public int TouchSortingLayerID
		{
			get
			{
				return touchSortingLayerID;
			}
			
			set
			{
				if(value != touchSortingLayerID)
				{
					touchSortingLayerID = value;
					
					AccordTouchLayerNameToID();
					
					TouchControllerScheduler.OnUpdateSorting(this);
				}
			}
		}
		
		public int TouchSortingOrder
		{
			get
			{
				return touchSortingOrder;
			}
			
			set
			{
				if(value != touchSortingOrder)
				{
					touchSortingOrder = value;
					TouchControllerScheduler.OnUpdateSorting(this);
				}
			}
		}
		
		public Vector2 StartTouchPosition
		{
			get
			{
				if(currentTouchInfos == null)
				{
					return Vector2.zero;
				}
				else
				{
					return currentTouchInfos.StartTouchPosition;
				}
			}

			set
			{
				if(currentTouchInfos == null)
					return;

				currentTouchInfos.StartTouchPosition = value;
			}
		}
		
		public Vector2 CurrentTouchPosition
		{
			get
			{
				if(currentTouchInfos == null)
				{
					return Vector3.zero;
				}
				else
				{
					return currentTouchInfos.CurrentTouchPosition;
				}
			}
		}
		
		public int CurrentTouchIndex
		{
			get
			{
				if(currentTouchInfos == null)
				{
					return -1;
				}
				else
				{
					return currentTouchInfos.TouchIndex;
				}
			}
		}
		
		public float TouchDistanceFromStart_Pixel
		{
			get
			{
				return (CurrentTouchPosition - StartTouchPosition).magnitude;
			}
		}
		
		public Vector3 TouchMovementFromStart_Pixel
		{
			get
			{
				return CurrentTouchPosition - StartTouchPosition;
			}
		}

		public float TouchDistanceFromStart_Centimeter
		{
			get
			{
				return TouchDistanceFromStart_Pixel * TouchUtility.PixelToCentimeter;
			}
		}
		
		public Vector3 TouchMovementFromStart_Centimeter
		{
			get
			{
				return TouchMovementFromStart_Pixel * TouchUtility.PixelToCentimeter;
			}
		}

		public bool IsSwiping
		{
			get
			{
				return swiping;
			}
		}
		
		public bool IsTouched
		{
			get
			{
				return touched;
			}
		}

		public void ForceUpdateController()
	    {
			FirstUpdateController();
			SecondUpdateController();
		}

		public void FirstUpdateController()
	    {	
			UpdateControlledButton();
			
			if(buttonEnabled)
			{
				FirstUpdateTouch();
			}
		}

	    public void SecondUpdateController()
	    {	
			UpdateControlledButton();
			
			if(buttonEnabled)
			{
				SecondUpdateTouch();
			}
			
			if(afterUpdate != null)
			{
				afterUpdate();
			}
		}

		void UpdateControlledButton()
		{
			bool bButtonEnabled;
			if(controlledButton == null)
			{
				bButtonEnabled = true;
			}
			else
			{
				bButtonEnabled = controlledButton.enabled;
			}
			
			if(bButtonEnabled != buttonEnabled)
			{
				if(bButtonEnabled)
				{
					OnButtonEnable();
				}
				else
				{
					OnButtonDisable();
				}
			}
		}

		public void CancelTouch()
		{
			// Cancel touch
			if(useCurrentTouch)
			{
				StopUseCurrentTouch();
			}
			touched = false;
			CancelButton();
			StopSwipe();
			
			touched = false;
			numberOfClick = 0;
			timeSinceLastClick = 0;
			
			buttonEnabled = false;
		}
		
		public void SendSwipeEvent(int a_iTouchIndex)
		{
			currentTouchInfos.TouchIndex = a_iTouchIndex;
			NotifySwipe(true);
		}
		
		public void ForceStartTouch(int a_iTouchIndex, bool a_bForceSwipe)
		{
			// Cancel current touch
			CancelTouch();
		
			// Try to start touch
			bool bTouchStarted = false;
			// Touches
			if(TouchUtility.MultiTouchEnabled)
			{
				foreach(Touch rTouch in Input.touches)
				{
					TouchPhase eTouchPhase = rTouch.phase;
					if(rTouch.fingerId == a_iTouchIndex && eTouchPhase != TouchPhase.Canceled && eTouchPhase != TouchPhase.Ended)
					{
						currentTouchInfos.StartTouch(ETouchInstigator.Finger, a_iTouchIndex, rTouch.position);
						bTouchStarted = true;
						break;
					}
				}
			}

			if(bTouchStarted == false)
			{
				// Mouse buttons
				if(Input.GetMouseButton(a_iTouchIndex) && clickFilter.mouseButtons.Contains(a_iTouchIndex))
				{
					currentTouchInfos.StartTouch(ETouchInstigator.Mouse, a_iTouchIndex, Input.mousePosition);
					bTouchStarted = true;
				}
			}
			
			if(bTouchStarted)
			{
				CancelAllOtherUsedTouches();
				StartTouch();
				if(a_bForceSwipe)
				{
					StartSwipe();
				}
			}
		}
		
		void Start()
		{
			AccordTouchLayerIDToName();
			manageByScheduler = TouchControllerScheduler.TryToRegister(this);
			
			if(touchStartMode == ETouchStartMode.MustTapButDontNeedToOnStart)
			{
				TryToTouch_DontNeedATap();
			}
		}
		
		void OnDestroy()
		{
			TouchControllerScheduler.TryToUnregister(this);
		}
		
		void OnEnable()
	    {
			OnButtonEnable();
		}
		
		void OnDisable()
		{
			OnButtonDisable();
		}
		
	    void Update()
	    {	
			if(manageByScheduler == false)
			{
				FirstUpdateController();
				SecondUpdateController();
			}
		}
		
		void OnButtonEnable()
	    {
			if(touchStartMode == ETouchStartMode.MustTapButDontNeedToOnEnable)
			{
				TryToTouch_DontNeedATap();
			}
			buttonEnabled = true;
		}
		
		void OnButtonDisable()
		{
			CancelTouch();
		}
		
		void PressButton()
		{
			if(controlledButton != null)
			{
				controlledButton.Press();
			}
		}
		
		void ReleaseButton()
		{
			if(controlledButton != null)
			{
				controlledButton.Release();
			}
		}
		
		void CancelButton()
		{
			if(controlledButton != null)
			{
				controlledButton.Cancel();
			}
		}
		
		void StartTouch()
		{
			switch(touchUsageMode)
			{
				case ETouchUsageMode.UseTouchExclusively:
				{
					StartUseCurrentTouch(true);	
				}
				break;
				
				case ETouchUsageMode.DontUseTouchExclusively:
				{
					StartUseCurrentTouch(false);
				}
				break;
			}
			
			hold.StartTouch();
			
			touched = true;
			touchHasBeenCanceled = false;
			
			PressButton();
		}
		
		void EndTouch()
		{		
			if(useCurrentTouch)
			{
				StopUseCurrentTouch();
			}
			
			touched = false;
			
			// Transfert the touch if needed
			switch(touchTransfertMode)
			{
				case ETouchTransfertMode.SelectAnOtherValidTouchOnTouchEnd:
				{
					if(CanTransfertCurrentTouch())
					{
						if(TryToTouch_DontNeedATap())
						{
							return;
						}
					}
				}
				break;
			}
			
			if(touchHasBeenCanceled == false && IsTouchASuccess(currentTouchInfos.CurrentTouchPosition))
			{
				if(otherTouchCancelMode == EOtherTouchCancelMode.CancelAllOtherTouchesOnRelease)
				{
					CancelAllOtherUsedTouches();
				}
				else if(otherTouchCancelMode == EOtherTouchCancelMode.CancelLesserPriorityTouchOnRelease)
				{
					CancelOtherUsedTouchesWithLesserPriorities();
				}

				ReleaseButton();
			}
			else
			{
				CancelButton();
			}
			
			StopSwipe();
		}
		
		bool CanTransfertCurrentTouch()
		{
			return touchEndType == ETouchEndType.Release;
		}
		
		bool TryToStartTheTouch()
		{			
			switch(touchStartMode)
			{
				case ETouchStartMode.MustTap:
				case ETouchStartMode.MustTapButDontNeedToOnEnable:
				case ETouchStartMode.MustTapButDontNeedToOnStart:
				{
					return TryToTouch_NeedATap();
				}
				
				case ETouchStartMode.DontNeedToTap:
				{
					return TryToTouch_DontNeedATap();
				}
			}
			return false;
		}

		void FirstUpdateTouch()
		{	
			ProcessClickCount();
			
			if(touched)
			{
				hold.UpdateTouch();
			}
			
			if(touched == false)
			{
				TryToStartTheTouch();
			}
		}

		void SecondUpdateTouch()
		{				
			if(touched)
			{	
				// Update the touch position if still active
				bool touchUpdateSucceeded = TryToUpdateCurrentTouchPosition();
				
				UpdateSwiping();
				
				if(touchUpdateSucceeded == false)
				{
					EndTouch();
				}
			}
		}
		
		void UpdateSwiping()
		{	
			bool bSwiping = IsTheCurrentTouchASwipe();
			if(bSwiping != swiping)
			{
				if(bSwiping)
				{
					StartSwipe();
				}
				else
				{
					StopSwipe();
				}
			}
		}
		
		void StartSwipe()
		{
			if(swiping == false)
			{
				swiping = true;
				NotifySwipe(swiping);
			}
		}
		
		void StopSwipe()
		{	
			if(swiping)
			{
				swiping = false;
				NotifySwipe(swiping);
			}
		}
		
		void NotifySwipe(bool a_bSwipe)
		{
			if(onSwipe != null)
			{
				onSwipe(swiping);
			}
		}
		
		bool TryToTouch_NeedATap()
		{
			if(TryToFindABeginningTouchInTheZone())
			{
				return StartTouchIfEnoughClick();
			}
			return false;
		}
		
		bool TryToTouch_DontNeedATap()
		{
			if(TryToFindATouchInTheZone())
			{
				return StartTouchIfEnoughClick();
			}
			return false;
		}
		
		bool StartTouchIfEnoughClick()
		{
			AddClick();
			if(EnoughClick())
			{
				StartTouch();
				return true;
			}
			return false;
		}
		
		bool TryToFindABeginningTouchInTheZone()
		{
			// Touches
			if(TouchUtility.MultiTouchEnabled)
			{
				foreach(Touch rTouch in Input.touches)
				{
					if(rTouch.phase == TouchPhase.Began)
					{
						if(TryToGetTheTouchIfInTheZone(ETouchInstigator.Finger, rTouch.fingerId, rTouch.position))
						{
							return true;
						}
					}
				}
			}
			
			// Mouse buttons
			foreach(int iMouseButton in clickFilter.mouseButtons)
			{
				if(Input.GetMouseButtonDown(iMouseButton))
				{
					if(TryToGetTheTouchIfInTheZone(ETouchInstigator.Mouse, iMouseButton, Input.mousePosition))
					{
						return true;
					}
				}
			}
			
			return false;
		}
		
		bool TryToFindATouchInTheZone()
		{
			// Touches
			if(TouchUtility.MultiTouchEnabled)
			{
				foreach(Touch rTouch in Input.touches)
				{
					if(rTouch.phase == TouchPhase.Moved || rTouch.phase == TouchPhase.Stationary)
					{
						if(TryToGetTheTouchIfInTheZone(ETouchInstigator.Finger, rTouch.fingerId, rTouch.position))
						{
							return true;
						}
					}
				}
			}
			
			// Mouse buttons
			foreach(int iMouseButton in clickFilter.mouseButtons)
			{
				if(Input.GetMouseButton(iMouseButton))
				{
					if(TryToGetTheTouchIfInTheZone(ETouchInstigator.Mouse, iMouseButton, Input.mousePosition))
					{
						return true;
					}
				}
			}
			
			return false;
		}
		
		bool CanTheTouchBeUsed(int a_iTouchIndex)
		{
			return TouchUsageManager.IsTouchUsedExclusively(a_iTouchIndex) == false;
		}
		
		void StartUseCurrentTouch(bool a_bExclusively)
		{
			TouchUsageManager.StartUseTouch(currentTouchInfos.TouchIndex, this, a_bExclusively);
			useCurrentTouch = true;
		}
		
		void StopUseCurrentTouch()
		{
			TouchUsageManager.StopUseTouch(currentTouchInfos.TouchIndex, this);
			useCurrentTouch = false;
		}
		
		void CancelOtherUsedTouchesWithLesserPriorities()
		{
			TouchUsageManager.CancelOtherUsedTouches(currentTouchInfos.TouchIndex, this);
		}
		
		void CancelAllOtherUsedTouches()
		{
			TouchUsageManager.CancelOtherUsedTouches(currentTouchInfos.TouchIndex, this, false);
		}
		
		bool TryToGetTheTouchIfInTheZone(ETouchInstigator a_eInstigator, int a_iTouchIndex, Vector2 a_f2TouchPosition)
		{
			if(IsInTheTouchZone(a_f2TouchPosition) && CanTheTouchBeUsed(a_iTouchIndex))
			{
				currentTouchInfos.StartTouch(a_eInstigator, a_iTouchIndex, a_f2TouchPosition);
				return true;
			}
			return false;
		}
		
		public bool IsInTheTouchZone(Vector2 a_f2TouchPosition)
		{
			return IsInTheTouchZone(a_f2TouchPosition, touchZones, true) 
				&& IsInTheTouchZone(a_f2TouchPosition, touchZonesExcluded, false) == false;
		}
		
		bool IsInTheTouchZone(Vector2 a_f2TouchPosition, List<TouchZoneBase> a_rTouchZones, bool a_bInfiniteZoneIfNoZones)
		{
			// If there isn't any touch zone we can touch anywhere in the screen
			if(a_rTouchZones.Count == 0)
			{
				return a_bInfiniteZoneIfNoZones;
			}
			else
			{
				// loop through the touch zones to see if we are touching at least one
				foreach(TouchZoneBase rTouchZone in a_rTouchZones)
				{
					if(rTouchZone == null)
					{
						return true;
					}
					else
					{
						if(rTouchZone.ContainsScreenPoint(a_f2TouchPosition))
						{
							return true;
						}
					}
				}
				return false;
			}
		}
		
		bool TryToUpdateCurrentTouchPosition()
		{
			if(currentTouchInfos.TryToUpdateTouchPosition())
			{
				if(IsTouchStillValidOnTouched(currentTouchInfos.CurrentTouchPosition))
				{
					if(HasTouchEnded(currentTouchInfos.CurrentTouchPosition) == false)
					{
						return true;
					}
				}
				else
				{
					touchHasBeenCanceled = true;
				}
			}
			else
			{
				touchEndType = ETouchEndType.Release;
			}
				
			return false;
		}
		
		bool IsTouchStillValidOnTouched(Vector2 a_f2TouchPosition)
		{
			switch(touchCancelationMode)
			{
				case ETouchCancelationMode.CancelWhenCursorOutOfZone:
				{
					if(IsInTheTouchZone(a_f2TouchPosition) == false)
					{
						touchEndType = ETouchEndType.Release;
						return false;	
					}
				}
				break;
				
				case ETouchCancelationMode.CancelWhenSwiped:
				{
					if(IsSwiping)
					{
						touchEndType = ETouchEndType.Swipe;
						return false;
					}
				}
				break;

				case ETouchCancelationMode.CancelWhenHold:
				{
					if(HasHeldTheCurrentTouchLongEnough())
					{
						touchEndType = ETouchEndType.Hold;
						return false;
					}
				}
				break;

				case ETouchCancelationMode.CancelWhenHoldAndSwipe:
				{
					if(IsSwiping)
					{
						touchEndType = ETouchEndType.Swipe;
						return false;
					}

					if(HasHeldTheCurrentTouchLongEnough())
					{
						touchEndType = ETouchEndType.Hold;
						return false;
					}
				}
				break;
			}
			
			return true;
		}
		
		bool HasTouchEnded(Vector2 a_f2TouchPosition)
		{		
			switch(touchEndMode)
			{
				case ETouchEndMode.EndOnSwipe:
				case ETouchEndMode.EndOnSwipeOrOnRelease:
				{
					if(IsSwiping)
					{
						touchEndType = ETouchEndType.Swipe;
						return true;
					}
				}
				break;
				
				case ETouchEndMode.EndOnHold:
				{
					if(HasHeldTheCurrentTouchLongEnough())
					{
						touchEndType = ETouchEndType.Hold;
						return true;
					}
				}
				break;
			}
			
			return false;
		}
		
		bool IsTouchASuccess(Vector2 a_f2TouchPosition)
		{	
			// Enough?
			switch(touchEndMode)
			{
				case ETouchEndMode.EndOnRelease:
				{
					if(touchCancelationMode == ETouchCancelationMode.CancelWhenReleaseOutOfZone && IsInTheTouchZone(a_f2TouchPosition) == false)
					{
						return false;
					}
				}
				break;
				
				case ETouchEndMode.EndOnHold:
				{
					if(touchEndType != ETouchEndType.Hold)
					{
						return false;
					}
				}
				break;
				
				case ETouchEndMode.EndOnSwipe:
				{
					if(touchEndType != ETouchEndType.Swipe)
					{
						return false;
					}
				}
				break;
				
				case ETouchEndMode.EndOnSwipeOrOnRelease:
				{
					if(touchCancelationMode == ETouchCancelationMode.CancelWhenReleaseOutOfZone && IsInTheTouchZone(a_f2TouchPosition) == false)
					{
						return false;
					}
				}
				break;
			}
			
			return true;
		}
		
		bool IsTheCurrentTouchASwipe()
		{
			if(touched)
			{
				return swipe.IsASwipe(currentTouchInfos.StartTouchPosition, currentTouchInfos.CurrentTouchPosition);
			}
			else
			{
				return false;
			}
		}
		
		bool HasHeldTheCurrentTouchLongEnough()
		{
			return hold.HasHeldLongEnough();
		}
		
		void AddClick()
		{
			numberOfClick++;
			timeSinceLastClick = 0.0f;
		}
		
		bool EnoughClick()
		{
			return numberOfClick >= clickCountRules.numberOfClickNeeded;
		}
		
		void ProcessClickCount()
		{
			if(timeSinceLastClick >= clickCountRules.clickRemanenceDuration)
			{
				numberOfClick = 0;
			}
			else
			{
				timeSinceLastClick += Time.deltaTime;
			}
		}
		
		void AccordTouchLayerIDToName()
		{
			touchSortingLayerID = TouchLayer.TouchLayerNameToID(touchSortingLayerName);
		}
		
		void AccordTouchLayerNameToID()
		{
			touchSortingLayerName = TouchLayer.TouchLayerIDToName(touchSortingLayerID);
		}
	}
}