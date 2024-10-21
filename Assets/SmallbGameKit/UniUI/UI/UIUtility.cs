using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniUI
{
	public static class UIUtility
	{
		public static Vector3 GameplayToCanvasWorldPosition_Clamped(Canvas canvas,
			Camera gameplayCamera,
			Vector3 gameplayPosition,
			RectTransform rectTransform)
		{
			Vector3 screenPoint = gameplayCamera.WorldToScreenPoint(gameplayPosition);

			Vector2 canvasLocalPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint,
				canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out canvasLocalPosition);

			Rect rect = rectTransform.rect;
			canvasLocalPosition.x = Mathf.Clamp(canvasLocalPosition.x, rect.min.x, rect.max.x);
			canvasLocalPosition.y = Mathf.Clamp(canvasLocalPosition.y, rect.min.y, rect.max.y);

			Vector3 canvasWorldPosition = rectTransform.TransformPoint(canvasLocalPosition);

			return canvasWorldPosition;
		}

		public static Vector3 GameplayToCanvasWorldPosition(Canvas canvas,
			Camera gameplayCamera,
			Vector3 gameplayPosition)
		{
			Vector3 screenPoint = gameplayCamera.WorldToScreenPoint(gameplayPosition);

			screenPoint.x = Mathf.Clamp(screenPoint.x, 0.0f, Screen.width);
			screenPoint.y = Mathf.Clamp(screenPoint.y, 0.0f, Screen.height);

			Vector3 canvasWorldPosition;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint,
				canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out canvasWorldPosition);

			if(screenPoint.z < 0.0f)
			{
				canvasWorldPosition.x *= -1.0f;
				canvasWorldPosition.y *= -1.0f;
			}

			return canvasWorldPosition;
		}

		public static Vector2 WorldToRectTransformLocalPosition(Canvas canvas, RectTransform parentRectTransform,
			Camera worldCamera,
			Vector3 worldPosition)
		 {
			 Vector3 screenPoint = worldCamera.WorldToScreenPoint(worldPosition);

			 Vector2 localPosition;
			 RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, screenPoint,
				 canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out localPosition);

			if(screenPoint.z < 0.0f)
			{
				localPosition.x *= -1.0f;
				localPosition.y *= -1.0f;
			}

			return localPosition;
		 }

		public static Vector2 RectTransformToRectTransformLocalPosition(RectTransform parentRectTransformFrom,
			RectTransform parentRectTransformTo,
			Vector3 localPosition)
		 {
			Vector3 worldPosition = parentRectTransformFrom.TransformPoint(localPosition);
			Vector3 newLocalPosition = parentRectTransformTo.InverseTransformPoint(worldPosition);

			return newLocalPosition;
		 }
	}
}
