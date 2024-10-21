using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniSkin
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/MaterialInstance")]
	public class MaterialInstance : MonoBehaviour
	{
		public Renderer rendererComponent;

		public Color color;

		public Material sharedMaterial;

		[SerializeField]
		Material instanciatedMaterial;

		public Color Color
		{
			get
			{
				return color;
			}

			set
			{
				if(value != color)
				{
					color = value;
					UpdateMaterial();
				}
			}
		}

		Material RendererSharedMaterial
		{
			get
			{
				if(rendererComponent == null)
					return null;

				return rendererComponent.sharedMaterial;
			}

			set
			{
				if(rendererComponent == null)
					return;

				rendererComponent.sharedMaterial = value;
			}
		}

		Material RendererInstanceMaterial
		{
			get
			{
				return rendererComponent.material;
			}

			set
			{
				rendererComponent.material = value;
			}
		}

		public void ResetToShareMaterial()
		{
			RestoreSharedMaterial();

			if(sharedMaterial != null)
				color = sharedMaterial.color;
		}

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			if(rendererComponent == null)
				return;

			if(sharedMaterial == null)
				return;

			UpdateMaterial();
		}
#endif

		void RestoreSharedMaterial()
		{
			RendererSharedMaterial = sharedMaterial;

			if(instanciatedMaterial != null)
			{
				DestroyImmediate(instanciatedMaterial);
				instanciatedMaterial = null;
			}
		}

		void UpdateMaterial()
		{
			if(color == sharedMaterial.color)
			{
				RestoreSharedMaterial();
				return;
			}

			if(instanciatedMaterial != null
				&& instanciatedMaterial != RendererSharedMaterial)
			{
				DestroyImmediate(instanciatedMaterial);
				instanciatedMaterial = null;
			}

			if(instanciatedMaterial == null)
			{
				RestoreSharedMaterial();
			}

			if(color != RendererSharedMaterial.color)
			{
				if(instanciatedMaterial == null)
				{
					#if UNITY_EDITOR
					if(Application.isPlaying == false)
					{
						instanciatedMaterial = Instantiate<Material>(sharedMaterial);
						RendererSharedMaterial = instanciatedMaterial;
					}
					else
					#endif
					{
						instanciatedMaterial = RendererInstanceMaterial;
					}
				}

				instanciatedMaterial.color = color;
			}
		}
	}
}
