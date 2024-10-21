using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkMeshGizmosUtility
	{	
		public static void DrawNormalsGizmos(Mesh a_oMesh, Color a_oColor, float a_fSize = 1.0f)
		{
			if(a_oMesh != null)
			{
				Color oColorSave = Gizmos.color;
				Gizmos.color = Color.red;
				
				Vector3[] oNormals = a_oMesh.normals;
				Vector3[] oVertices = a_oMesh.vertices;
				
				int iVertexCount = a_oMesh.vertexCount;
				for(int i = 0; i < iVertexCount; i++)
				{
					Vector3 f3Vertex = oVertices[i];
					Gizmos.DrawLine(f3Vertex, f3Vertex + oNormals[i] * a_fSize); 
				}
				
				Gizmos.color = oColorSave;
			}
		}
	}
}