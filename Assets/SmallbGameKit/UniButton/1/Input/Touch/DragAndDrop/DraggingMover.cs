using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/DragAndDrop/DraggingMover")]
	public class DraggingMover : MonoBehaviour
	{
		public DraggingHandle draggingHandle;
		
		public Transform transformToDrag;
		
		public Camera draggingMovementCamera;
		
		Vector2 dragOffset;
		
		Vector3 draggingPosition;
		
		public Camera DraggingMovementCamera
		{
			get
			{
				return draggingMovementCamera;
			}
		}
		
		public bool IsDragging
		{
			get
			{
				return draggingHandle.IsDragging;
			}
		}
		
		void Start()
		{
			if(draggingMovementCamera == null)
			{
				draggingMovementCamera = Camera.main;
			}
			draggingHandle.onStartDragging += OnStartDragging;
			if(draggingHandle.IsDragging)
			{
				OnStartDragging(draggingHandle);
			}
		}
		
		void OnDestroy()
		{
			draggingHandle.onStartDragging -= OnStartDragging;
		}
		
		void LateUpdate()
		{
			if(draggingHandle.IsDragging)
			{
				UpdateDragging();
			}
		}
		
		void OnStartDragging(DraggingHandle a_rDraggingHandle)
		{
			Vector2 f2TransformScreenPosition = draggingMovementCamera.WorldToScreenPoint(transformToDrag.position);
			dragOffset = a_rDraggingHandle.DraggingStartPosition - f2TransformScreenPosition;
		}
		
		void UpdateDragging()
		{
			Vector3 f3NewPosition = ComputeWorldDraggingPosition(draggingHandle.DraggingPosition, -dragOffset);
			transformToDrag.position = f3NewPosition;
		}
		
		Vector3 ComputeWorldDraggingPosition(Vector2 a_f2Position, Vector2 a_f2Offset)
		{
			float fDepth = draggingMovementCamera.WorldToScreenPoint(transformToDrag.position).z;
			
			Vector3 f3DraggingPosition = a_f2Position + a_f2Offset;
			f3DraggingPosition.z = fDepth;
			f3DraggingPosition = draggingMovementCamera.ScreenToWorldPoint(f3DraggingPosition);
			
			return f3DraggingPosition;
		}
	}
}