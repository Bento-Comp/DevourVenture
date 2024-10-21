using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkMeshCopyUtility
	{	
		public static Mesh CopyMesh(Mesh a_rMeshToCopy)
		{
			Mesh rCopiedMesh = new Mesh();
		
			CopyMesh(a_rMeshToCopy, ref rCopiedMesh);
			
			return rCopiedMesh;
		}
		
		public static void CopyMesh(Mesh a_rFrom, ref Mesh a_rTo)
		{
			a_rTo.Clear();
			
			if(a_rFrom == null)
			{
				return;
			}
	
			a_rTo.vertices = a_rFrom.vertices;
			a_rTo.uv = a_rFrom.uv;
			a_rTo.uv2 = a_rFrom.uv2;
			a_rTo.colors32 = a_rFrom.colors32;
			a_rTo.tangents = a_rFrom.tangents;
			a_rTo.normals = a_rFrom.normals;
	
			a_rTo.boneWeights = a_rFrom.boneWeights;
			a_rTo.bindposes = a_rFrom.bindposes;
			a_rTo.bounds = a_rFrom.bounds;
	
			a_rTo.name = a_rFrom.name;
		
			// Iterate submeshes to copy their topology (or just triangles before unity 4)
			int iSubMeshCount      = a_rFrom.subMeshCount;
			a_rTo.subMeshCount  = iSubMeshCount;
			for( int iSubMeshIndex = 0; iSubMeshIndex < iSubMeshCount; ++iSubMeshIndex )
			{
				#if UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
				a_rTo.SetTriangles( a_rFrom.GetTriangles( iSubMeshIndex ), iSubMeshIndex );
				#else
				a_rTo.SetIndices( a_rFrom.GetIndices( iSubMeshIndex ), a_rFrom.GetTopology( iSubMeshIndex ), iSubMeshIndex );
				#endif
			}
	
			a_rTo.RecalculateBounds();
			;
		}
	}
}