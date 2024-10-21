using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class gkMeshColorUtility
	{	
		static public void SetVertexColor(Mesh a_rMesh, Color a_rColor)
		{
			// Get
			Color[] oColors = a_rMesh.colors;
			if(oColors == null || oColors.Length < a_rMesh.vertexCount)
			{
				oColors = new Color[a_rMesh.vertexCount];
			}
				
			// Modify
			for(int i = 0; i < oColors.Length; i++)
			{
				oColors[i] = a_rColor;
			}
			
			// Set
			a_rMesh.colors = oColors;
		}
		
		static public void SetVertexColor(Mesh a_rMesh, Color a_rColor, Color[] a_rVertexColors)
		{
			// Modify
			for(int i = 0; i < a_rVertexColors.Length; i++)
			{
				a_rVertexColors[i] = a_rColor;
			}
			
			// Set
			a_rMesh.colors = a_rVertexColors;
		}
		
		static public void SetVertexColorAlpha(Mesh a_rMesh, float a_fAlpha)
		{
			if(a_rMesh != null)
			{
				Color[] oVertexColors = a_rMesh.colors;
				
				int iVertexCount = a_rMesh.vertexCount;
				if(oVertexColors.Length != iVertexCount)
				{
					oVertexColors = new Color[iVertexCount];
					for(int i = 0; i < iVertexCount; i++)
					{
						oVertexColors[i] = Color.white;
					}
				}
				
				// Modify
				for(int i = 0; i < oVertexColors.Length; i++)
				{
					oVertexColors[i].a = a_fAlpha;
				}
				
				// Set
				a_rMesh.colors = oVertexColors;
			}
		}
		
		static public void SetVertexColorIntensity(Mesh a_rMesh, float a_fIntensity)
		{
			if(a_rMesh != null)
			{
				Color[] oVertexColors = a_rMesh.colors;
				
				// Modify
				for(int i = 0; i < oVertexColors.Length; i++)
				{
					Color oColor = oVertexColors[i];
					
					oColor.r = a_fIntensity;
					oColor.g = a_fIntensity;
					oColor.b = a_fIntensity;
					
					oVertexColors[i] = oColor;
				}
				
				// Set
				a_rMesh.colors = oVertexColors;
			}
		}
		
		static public Color[] CreateVertexColorBuffer(Mesh a_rMesh)
		{
			return new Color[a_rMesh.vertexCount];
		}
	}
}