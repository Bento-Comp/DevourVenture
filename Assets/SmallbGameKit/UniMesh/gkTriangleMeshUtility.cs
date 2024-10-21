using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public enum ETriangleType
	{
		Isoceles,
		Equilateral
	}
	
	public static class gkTriangleMeshUtility
	{	
		public static Mesh CreateTriangleMesh(ETriangleType a_eTriangleType, EMeshCreationAxis a_eAxis = EMeshCreationAxis.Back)
		{
			return CreateTriangleMesh(a_eTriangleType, a_eAxis, Vector2.one, Vector3.zero);
		}
		
		public static Mesh CreateTriangleMesh(ETriangleType a_eTriangleType, EMeshCreationAxis a_eAxis, Vector2 a_f2LocalSize)
		{
			return CreateTriangleMesh(a_eTriangleType, a_eAxis, a_f2LocalSize, Vector3.zero);
		}
		
		public static Mesh CreateTriangleMesh(ETriangleType a_eTriangleType, EMeshCreationAxis a_eAxis, Vector2 a_f2LocalSize, Vector3 a_f3LocalOffset)
		{
			Mesh oMesh = new Mesh();
			MakeIntoATriangleMesh(a_eTriangleType, oMesh, a_eAxis, a_f2LocalSize, a_f3LocalOffset);
			
			return oMesh;
		}
		
		public static void MakeIntoATriangleMesh(ETriangleType a_eTriangleType, Mesh a_oMesh, EMeshCreationAxis a_eAxis = EMeshCreationAxis.Back)
		{
			MakeIntoATriangleMesh(a_eTriangleType, a_oMesh, a_eAxis, Vector2.one, Vector3.zero);
		}
		
		public static void MakeIntoATriangleMesh(ETriangleType a_eTriangleType, Mesh a_oMesh, EMeshCreationAxis a_eAxis, Vector2 a_f2LocalSize)
		{
			MakeIntoATriangleMesh(a_eTriangleType, a_oMesh, a_eAxis, a_f2LocalSize, Vector3.zero);
		}
		
		public static void MakeIntoATriangleMesh(ETriangleType a_eTriangleType, Mesh a_oMesh, EMeshCreationAxis a_eAxis, Vector2 a_f2LocalSize, Vector3 a_f3LocalOffset)
		{
			Vector3[] oBaseVectors = gkMeshCreationUtility.GetMeshCreationAxisVectors(a_eAxis);
			
			Vector2 f2HalfSize = a_f2LocalSize;
			f2HalfSize *= 0.5f;
			
			Vector3[] oVertices;
			switch(a_eTriangleType)
			{
				case ETriangleType.Isoceles:
				{
					oVertices = new Vector3[]
					{
						(- oBaseVectors[0] * f2HalfSize.x) + (- oBaseVectors[1] * f2HalfSize.y),
															 (  oBaseVectors[1] * f2HalfSize.y),
						(  oBaseVectors[0] * f2HalfSize.x) + (- oBaseVectors[1] * f2HalfSize.y)
					};
				}
				break;
				
				case ETriangleType.Equilateral:
				default:
				{
					float fAngle = Mathf.PI/6.0f;
					float fCos = Mathf.Cos(fAngle);
					float fSin = Mathf.Sin(fAngle);
					oVertices = new Vector3[]
					{
						(- oBaseVectors[0] * f2HalfSize.x) * fCos + (- oBaseVectors[1] * f2HalfSize.y) * fSin,
															 (  oBaseVectors[1] * f2HalfSize.y),
						(  oBaseVectors[0] * f2HalfSize.x) * fCos + (- oBaseVectors[1] * f2HalfSize.y) * fSin
					};
				}
				break;
			}
			a_oMesh.vertices = oVertices;
				
			a_oMesh.triangles = new int[]
			{
				0, 1, 2,
			};
			
			a_oMesh.uv = new Vector2[]
			{
				new Vector2(0.0f, 0.0f),
				new Vector2(0.5f, 1.0f),
				new Vector2(1.0f, 0.0f)
			};
			
			a_oMesh.RecalculateNormals();
			a_oMesh.RecalculateBounds();
		}
	}
}