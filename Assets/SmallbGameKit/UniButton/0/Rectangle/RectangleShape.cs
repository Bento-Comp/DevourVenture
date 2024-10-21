using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace UniButton
{
	[AddComponentMenu("UniButton/Rectangle/RectangleShape")]
	public class RectangleShape : MonoBehaviour
	{
		public Rectangle rectangle = new Rectangle();
		
		public Color gizmoColor = Color.red;
		
		public Vector2 LocalSize
		{
			get
			{
				return rectangle.Size;
			}
			
			set
			{
				rectangle.Size = value;
			}
		}
		
		public Vector2 LossyWorldSize
		{
			get
			{
				Vector2 f2LossyScale = transform.lossyScale;
				return new Vector2(rectangle.width * f2LossyScale.x, rectangle.height * f2LossyScale.y);
			}
		}

		public bool Project(Camera a_rCamera, Vector2 a_f2ScreenPoint, out Vector3 projectionInWorldSpace)
		{
			return rectangle.Project(transform, a_rCamera, a_f2ScreenPoint, out projectionInWorldSpace);
		}

		public bool Contains(Camera a_rCamera, Vector2 a_f2ScreenPoint)
		{
			return rectangle.Contains(transform, a_rCamera, a_f2ScreenPoint);
		}
		
		public List<Vector3> GetLocalVertices()
		{
			return rectangle.GetLocalVertices();
		}
		
		public List<Vector3> GetWorldVertices()
		{
			return rectangle.GetWorldVertices(transform);
		}
		
		public List<Vector3> GetScreenAlignedVertices(Camera a_rCamera)
		{
			return rectangle.GetScreenAlignedVertices(transform, a_rCamera);
		}
		
		public Rect GetScreenAlignedRectangle(Camera a_rCamera)
		{
			return rectangle.GetScreenAlignedRectangle(transform, a_rCamera);
		}
		
		public Rect GetViewportAlignedRectangle(Camera a_rCamera)
		{
			return rectangle.GetViewportAlignedRectangle(transform, a_rCamera);
		}
		
		void OnDrawGizmos()
		{
			if(rectangle != null)
			{
				rectangle.DisplayLinesGizmos(transform, gizmoColor);
			}
		}
	}
}