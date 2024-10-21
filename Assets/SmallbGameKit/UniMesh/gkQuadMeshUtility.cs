using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkQuadMeshUtility
	{	
		public static Mesh CreateQuadMesh(EMeshCreationAxis a_eAxis = EMeshCreationAxis.Back)
		{
			return CreateQuadMesh(a_eAxis, Vector2.one, Vector3.zero);
		}
		
		public static Mesh CreateQuadMesh(EMeshCreationAxis a_eAxis, Vector2 a_f2LocalSize)
		{
			return CreateQuadMesh(a_eAxis, a_f2LocalSize, Vector3.zero);
		}
		
		public static Mesh CreateQuadMesh(EMeshCreationAxis a_eAxis, Vector2 a_f2LocalSize, Vector3 a_f3LocalOffset)
		{
			Mesh oMesh = new Mesh();
			MakeIntoAQuadMesh(oMesh, a_eAxis, a_f2LocalSize, a_f3LocalOffset);
			
			return oMesh;
		}
		
		public static void MakeIntoAQuadMesh(Mesh a_oMesh, EMeshCreationAxis a_eAxis = EMeshCreationAxis.Back)
		{
			MakeIntoAQuadMesh(a_oMesh, a_eAxis, Vector2.one, Vector3.zero);
		}
		
		public static void MakeIntoAQuadMesh(Mesh a_oMesh, EMeshCreationAxis a_eAxis, Vector2 a_f2LocalSize)
		{
			MakeIntoAQuadMesh(a_oMesh, a_eAxis, a_f2LocalSize, Vector3.zero);
		}
		
		public static void MakeIntoAQuadMesh(Mesh a_oMesh, EMeshCreationAxis a_eAxis, Vector2 a_f2LocalSize, Vector3 a_f3LocalOffset)
		{
			Vector3[] oBaseVectors = gkMeshCreationUtility.GetMeshCreationAxisVectors(a_eAxis);
			
			Vector2 f2HalfSize = a_f2LocalSize;
			f2HalfSize *= 0.5f;
			
			a_oMesh.vertices = new Vector3[]
			{
				(- oBaseVectors[0] * f2HalfSize.x) + (- oBaseVectors[1] * f2HalfSize.y),
				(- oBaseVectors[0] * f2HalfSize.x) + (  oBaseVectors[1] * f2HalfSize.y),
				(  oBaseVectors[0] * f2HalfSize.x) + (  oBaseVectors[1] * f2HalfSize.y),
				(  oBaseVectors[0] * f2HalfSize.x) + (- oBaseVectors[1] * f2HalfSize.y)
			};
			
			a_oMesh.triangles = new int[]
			{
				0, 1, 2,
				0, 2, 3
			};
			
			a_oMesh.uv = new Vector2[]
			{
				new Vector2(0.0f, 0.0f),
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 0.0f)
			};
			
			a_oMesh.RecalculateNormals();
			a_oMesh.RecalculateBounds();
		}
	}
}