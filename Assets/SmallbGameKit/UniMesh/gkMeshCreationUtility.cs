using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public enum EMeshCreationAxis
	{
		Up,
		Down,
		Forward,
		Back,
		Right,
		Left
	}
	
	public static class gkMeshCreationUtility
	{	
		public static Vector3 GetMeshCreationAxisVector(EMeshCreationAxis a_eAxis)
		{
			switch(a_eAxis)
			{
				case EMeshCreationAxis.Up:
				{
					return Vector3.up;
				}
				
				case EMeshCreationAxis.Down:
				{
					return Vector3.down;
				}
				
				case EMeshCreationAxis.Forward:
				{
					return Vector3.forward;
				}
				
				case EMeshCreationAxis.Back:
				{
					return Vector3.back;
				}
				
				case EMeshCreationAxis.Right:
				{
					return Vector3.right;
				}
				
				case EMeshCreationAxis.Left:
				{
					return Vector3.left;
				}
				
				default:
				{
					return Vector3.up;
				}
			}
		}
		
		public static Vector3[] GetMeshCreationAxisVectors(EMeshCreationAxis a_eAxis)
		{
			Vector3[] oVectors = new Vector3[3];
			
			switch(a_eAxis)
			{
				case EMeshCreationAxis.Up:
				{
					oVectors[0] = GetMeshCreationAxisVector(EMeshCreationAxis.Right);
					oVectors[1] = GetMeshCreationAxisVector(EMeshCreationAxis.Forward);
					oVectors[2] = GetMeshCreationAxisVector(EMeshCreationAxis.Up);
				}
				break;
				
				case EMeshCreationAxis.Down:
				{
					oVectors[0] = GetMeshCreationAxisVector(EMeshCreationAxis.Left);
					oVectors[1] = GetMeshCreationAxisVector(EMeshCreationAxis.Forward);
					oVectors[2] = GetMeshCreationAxisVector(EMeshCreationAxis.Down);
				}
				break;
				
				case EMeshCreationAxis.Forward:
				{
					oVectors[0] = GetMeshCreationAxisVector(EMeshCreationAxis.Left);
					oVectors[1] = GetMeshCreationAxisVector(EMeshCreationAxis.Up);
					oVectors[2] = GetMeshCreationAxisVector(EMeshCreationAxis.Forward);
				}
				break;
				
				case EMeshCreationAxis.Back:
				{
					oVectors[0] = GetMeshCreationAxisVector(EMeshCreationAxis.Right);
					oVectors[1] = GetMeshCreationAxisVector(EMeshCreationAxis.Up);
					oVectors[2] = GetMeshCreationAxisVector(EMeshCreationAxis.Back);
				}
				break;
				
				case EMeshCreationAxis.Right:
				{
					oVectors[0] = GetMeshCreationAxisVector(EMeshCreationAxis.Forward);
					oVectors[1] = GetMeshCreationAxisVector(EMeshCreationAxis.Up);
					oVectors[2] = GetMeshCreationAxisVector(EMeshCreationAxis.Right);
				}
				break;
				
				case EMeshCreationAxis.Left:
				{
					oVectors[0] = GetMeshCreationAxisVector(EMeshCreationAxis.Back);
					oVectors[1] = GetMeshCreationAxisVector(EMeshCreationAxis.Up);
					oVectors[2] = GetMeshCreationAxisVector(EMeshCreationAxis.Left);
				}
				break;
				
				default:
				{
					GetMeshCreationAxisVectors(EMeshCreationAxis.Up);
				}
				break;
			}
			
			return oVectors;
		}
	}
}