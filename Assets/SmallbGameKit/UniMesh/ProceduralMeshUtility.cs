using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gk
{
	public static class ProceduralMeshUtility
	{	
		public static void AddQuad(List<int> triangles, int offset = 0, bool flip = false)
		{
			AddQuad(triangles, 0, 1, 2, 3, offset, flip);
		}

		public static void AddQuad(int[] triangles, ref int startIndex, int offset = 0, bool flip = false)
		{
			AddQuad(triangles, ref startIndex, 0, 1, 2, 3, offset, flip);
		}

		public static void AddQuad(List<int> triangles, int a, int b, int c, int d, int offset = 0, bool flip = false)
		{
			AddTriangle(triangles, a, b, d, offset, flip);
			AddTriangle(triangles, d, b, c, offset, flip);
		}

		public static void AddQuad(int[] triangles, ref int startIndex, int a, int b, int c, int d, int offset = 0, bool flip = false)
		{
			AddTriangle(triangles, ref startIndex, a, b, d, offset, flip);
			AddTriangle(triangles, ref startIndex, d, b, c, offset, flip);
		}

		public static void AddTriangle(List<int> triangles, int a, int b, int c, int offset = 0, bool flip = false)
		{
			a += offset;
			b += offset;
			c += offset;

			if(flip)
			{
				triangles.Add(a);
				triangles.Add(c);
				triangles.Add(b);
			}
			else
			{
				triangles.Add(a);
				triangles.Add(b);
				triangles.Add(c);
			}
		}

		public static void AddTriangle(int[] triangles, ref int startIndex, int a, int b, int c, int offset = 0, bool flip = false)
		{
			a += offset;
			b += offset;
			c += offset;

			if(flip)
			{
				triangles[startIndex + 0] = a;
				triangles[startIndex + 1] = c;
				triangles[startIndex + 2] = b;
			}
			else
			{
				triangles[startIndex + 0] = a;
				triangles[startIndex + 1] = b;
				triangles[startIndex + 2] = c;
			}

			startIndex += 3;
		}
	}
}