using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniCurves
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniCurves/BezierCurve_2DMesh_HeightSurface")]
	public class BezierCurve_2DMesh_HeightSurface : MonoBehaviour
	{
		public enum EVertexColorMode
		{
			None,
			Uniform,
			Gradient
		}

		public BezierCurve bezierCurve;

		public float bottomHeight;

		public bool flip;

		public float skinThickness = 0.0f;

		public EVertexColorMode vertexColorMode;

		public Color uniformColor = Color.white;

		public Color skinColor = Color.white;

		public Color topColor = Color.white;

		public Color bottomColor = Color.black;

		MeshFilter meshFilter;

		Mesh mesh;

		void Awake()
		{
			GetComponents();

			UpdateMesh();
		}

		void OnEnable()
		{
			UpdateMesh();
		}

		void Update()
		{
			GetComponents();

			UpdateMesh();
		}

		void UpdateMesh()
		{
			if(bezierCurve == null || meshFilter == null)
				return;

			if(mesh == null)
			{
				mesh = new Mesh();
			}
			else
			{
				mesh.Clear();
			}

			List<Vector3> vertices = bezierCurve.ComputeVertices();

			bool useSkin = skinThickness != 0.0f;
			Vector3 skinOffset = Vector3.up * skinThickness;

			// Vertices
			int verticesCount = vertices.Count;
			int meshVerticesCount;
			if(useSkin)
			{
				meshVerticesCount = verticesCount  * 3;
			}
			else
			{
				meshVerticesCount = verticesCount  * 2;
			}
			Vector3[] meshVertices = new Vector3[meshVerticesCount];
			for(int i = 0; i < verticesCount; ++i)
			{
				Vector3 top = transform.InverseTransformPoint(vertices[i]);

				Vector3 bottom = top;
				bottom.y = bottomHeight;

				if(useSkin)
				{
					Vector3 skinTop;
					Vector3 skinBottom;
					if(skinThickness > 0.0f)
					{
						skinTop = top + skinOffset;
						skinBottom = top;
					}
					else
					{
						skinTop = top;
						skinBottom = top + skinOffset;
					}

					meshVertices[i] = skinTop;
					meshVertices[i + verticesCount] = skinBottom;
					meshVertices[i + verticesCount * 2] = bottom;
				}
				else
				{
					meshVertices[i] = top;
					meshVertices[i + verticesCount] = bottom;
				}
			}

			// Triangles
			int meshTrianglesCount;
			if(useSkin)
			{
				meshTrianglesCount = (vertices.Count - 1) * 2 * 3 * 2;
			}
			else
			{
				meshTrianglesCount = (vertices.Count - 1) * 2 * 3;
			}
			int[] meshTriangles = new int[meshTrianglesCount];
			int currentIndexOffset = 0;
			for(int i = 0; i < vertices.Count - 1; ++i)
			{
				gk.ProceduralMeshUtility.AddQuad(meshTriangles, ref currentIndexOffset, i, i + 1, i + 1 + verticesCount, i + verticesCount, 0, flip);
				if(useSkin)
				{
					gk.ProceduralMeshUtility.AddQuad(meshTriangles, ref currentIndexOffset, i + verticesCount, i + 1 + verticesCount, i + 1 + 2 * verticesCount, i + 2 * verticesCount, 0, flip);
				}
			}

			mesh.vertices = meshVertices;
			mesh.triangles = meshTriangles;

			// Color
			if(vertexColorMode != EVertexColorMode.None)
			{
				Color32[] meshColors = new Color32[meshVertices.Length];
				switch(vertexColorMode)
				{
					case EVertexColorMode.Uniform:
					{
						for(int i = 0; i < meshColors.Length; ++i)
						{
							meshColors[i] = uniformColor;
						}
					}
					break;

					case EVertexColorMode.Gradient:
					{
						for(int i = 0; i < verticesCount; ++i)
						{
							if(useSkin)
							{
								meshColors[i] = skinColor;
								meshColors[i + verticesCount ] = topColor;
								meshColors[i + verticesCount * 2] = bottomColor;
							}
							else
							{
								meshColors[i] = topColor;
								meshColors[i + verticesCount ] = bottomColor;
							}
						}
					}
					break;
				}

				mesh.colors32 = meshColors;
			}

			mesh.RecalculateNormals();

			mesh.name = bezierCurve.name + "_HeightSurface";

			meshFilter.sharedMesh = mesh;
		}

		void GetComponents()
		{
			meshFilter = GetComponent<MeshFilter>();
		}
	}
}