using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniButton;

namespace UniButton
{
	[AddComponentMenu("UniButton/TouchButtonController_Stick")]
	public class TouchButtonController_Stick : MonoBehaviour
	{
		public UniButton.TouchButtonController touchButtonController;

		public float amplitudeMax = 0.5f;

		public bool clampToAmplitude = true;

		public bool keepTightStick = true;

		public bool gizmo_enable;

		public float gizmo_knobSize_centimeters = 0.15f;

		public bool squareStick;

		public bool useCounterSteer;

		public float counterSteerReduction = 0.5f;
		public float counterSteerDeadZone = 0.1f;

		public bool useStickComeback;

		public float comebackStickGravity = 1.0f;

		public AnimationCurve comebackByDistanceCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

		public float comebackStickDrag = 0.0f;

		public float comebackStickSmoothTime = 0.2f;

		public float debug_comebackStickCurrentVelocityFloat;

		float comebackStickCurrentVelocityFloat;
		Vector2 comebackStickCurrentVelocity;

		float lastSteer = 0.0f;

		Button button;

		public Vector2 Stick {get; private set;}

		public bool Pressed => touchButtonController.controlledButton.Pressed;

		public void MoveStickCenter(Vector2 movementInStickPercent)
		{
			Vector2 movementInPixel = movementInStickPercent * amplitudeMax * TouchUtility.CentimetersToPixel;

			touchButtonController.StartTouchPosition += movementInPixel;
		}

		void Awake()
		{
			button = touchButtonController.controlledButton;
		}

		void Update()
		{
			UpdateStick();
		}

		void UpdateStick()
		{
			if(button.Pressed == false)
			{
				Stick = Vector2.zero;
				return;
			}

			// Keep a tight stick
			if(keepTightStick)
			{
				Vector2 currentStickDirection = touchButtonController.TouchMovementFromStart_Centimeter.normalized;
				float currentStickDistance = touchButtonController.TouchMovementFromStart_Centimeter.magnitude;
				float maxStartStickDistance = amplitudeMax;
				if(currentStickDistance >= maxStartStickDistance)
				{
					touchButtonController.StartTouchPosition = touchButtonController.CurrentTouchPosition
						- currentStickDirection * maxStartStickDistance * TouchUtility.CentimetersToPixel;
				}
			}

			// Counter steer
			if(useCounterSteer)
			{
				Vector2 currentStickVector = touchButtonController.TouchMovementFromStart_Centimeter;

				float steer = currentStickVector.x / amplitudeMax;

				if(Mathf.Abs(steer) <= counterSteerDeadZone)
				{
				}
				else
				{
					float steerMovement = steer - lastSteer;

					// If Counter steer
					if(Mathf.Sign(steerMovement * steer) == -1.0f)
					{
						steerMovement *= counterSteerReduction;
						steer = lastSteer + steerMovement;
					}

					currentStickVector.x = steer * amplitudeMax;

					touchButtonController.StartTouchPosition = touchButtonController.CurrentTouchPosition
								- currentStickVector * TouchUtility.CentimetersToPixel;
				}

				lastSteer = steer;
			}

			// Comeback
			if(useStickComeback)
			{
				Vector2 currentStickVector = touchButtonController.TouchMovementFromStart_Centimeter;
				Vector2 newStickVector = currentStickVector;

				float currentStickDistance = currentStickVector.magnitude;

				if(currentStickDistance > 0.0f)
				{
					Vector2 currentStickDirection = currentStickVector / currentStickDistance;
					Vector2 comebackDirection = -currentStickDirection;


					float gravity = comebackByDistanceCurve.Evaluate(currentStickDistance) * comebackStickGravity;

					comebackStickCurrentVelocity += comebackDirection * gravity * Time.deltaTime;

					comebackStickCurrentVelocity =
						UniUtilities.PhysicsUtility.ApplyDrag(comebackStickCurrentVelocity, comebackStickDrag, Time.deltaTime, 0.01f);

					Vector2 comebackMovementThisFrame = Time.deltaTime * comebackStickCurrentVelocity;

					newStickVector = currentStickVector + comebackMovementThisFrame;
				}

				touchButtonController.StartTouchPosition = touchButtonController.CurrentTouchPosition
							- newStickVector * TouchUtility.CentimetersToPixel;

				debug_comebackStickCurrentVelocityFloat = comebackStickCurrentVelocityFloat;
			}

			// Compute stick
			Vector2 stickVector = touchButtonController.TouchMovementFromStart_Centimeter;

			float amplitude = stickVector.magnitude;

			if(amplitude <= 0.0f)
			{
				stickVector = Vector2.zero;
			}
			else if(clampToAmplitude)
			{
				if(squareStick)
				{
					stickVector /= amplitudeMax;
					stickVector.x = Mathf.Clamp(stickVector.x, -1.0f, 1.0f);
					stickVector.y = Mathf.Clamp(stickVector.y, -1.0f, 1.0f);
				}
				else
				{
					if(amplitude >= amplitudeMax)
					{
						stickVector /= amplitude;
					}
					else
					{
						stickVector /= amplitudeMax;
					}
				}
			}
			else
			{
				stickVector /= amplitudeMax;
			}

			Stick = stickVector;
		}

		void OnDrawGizmos()
		{
			Camera mainCamera = Camera.main;

			if(mainCamera == null)
				return;

			if(touchButtonController == null)
				return;

			float depth = Mathf.Lerp(mainCamera.nearClipPlane, mainCamera.farClipPlane, 0.5f);

			float pixelToWorld = (mainCamera.ScreenToWorldPoint(new Vector3(0.0f, 1.0f, depth)) - mainCamera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, depth))).magnitude;

			Vector3 startTouch = touchButtonController.StartTouchPosition;
			Vector3 currentTouch = touchButtonController.CurrentTouchPosition;

			Vector3 start = mainCamera.ScreenToWorldPoint(new Vector3(touchButtonController.StartTouchPosition.x, touchButtonController.StartTouchPosition.y, depth));
			Vector3 current = mainCamera.ScreenToWorldPoint(new Vector3(touchButtonController.CurrentTouchPosition.x, touchButtonController.CurrentTouchPosition.y, depth));

			float amplitudeWorld = amplitudeMax * TouchUtility.CentimetersToPixel * pixelToWorld;

			Vector3 stickEnd = start + (Vector3)Stick * amplitudeWorld;

			Gizmos.color = Color.cyan;

			Gizmos.DrawWireSphere(start, amplitudeWorld);

			float distance = (current - start).magnitude;
			float epsilon = 1.0f * pixelToWorld;
			if(distance >= (amplitudeWorld - epsilon))
			{
				Vector3 intercept = (current - start).normalized * amplitudeWorld + start;

				Gizmos.DrawLine(start, intercept);

				Gizmos.color = Color.red;

				Gizmos.DrawLine(intercept, current);
			}
			else
			{
				Gizmos.DrawLine(start, current);
			}

			Gizmos.DrawWireSphere(current, gizmo_knobSize_centimeters * TouchUtility.CentimetersToPixel * pixelToWorld);

			Gizmos.color = Color.white;

			Gizmos.DrawLine(start, stickEnd);
			Gizmos.DrawWireSphere(stickEnd, gizmo_knobSize_centimeters * 0.5f * TouchUtility.CentimetersToPixel * pixelToWorld);
		}

		static bool IsMovementCrossingTarget(Vector2 start, float movementLength, Vector2 target, float reachTargetRadius)
		{
			float distanceToTarget = Vector2.Distance(start, target);
			return movementLength >= distanceToTarget - reachTargetRadius;
		}
	}
}