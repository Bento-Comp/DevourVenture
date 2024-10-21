using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniMaterial
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniMaterial/EmissiveIntensityHook")]
	public class EmissiveIntensityHook : MonoBehaviour 
	{
		public bool autoSetRendererWhenNull = true;

		public Renderer rendererComponent;

		public string hookTag = "";

		public string emissionColor_ShaderPropertyName = "_EmissionColor";

		public bool forceUseOtherMaterial;

		public Material initialMaterial;

		public Material forcedOtherMaterial;

		public int materialIndex;

		Color baseEmissiveColor;

		float emissionPercent;

		public float EmissionPercent
		{
			get => emissionPercent;

			set
			{
				emissionPercent = value;

				OnEmissionPercentChange();
			}
		}

		public Color BaseEmissiveColor
		{
			get
			{
				return baseEmissiveColor;
			}
		}

		public Material SharedMaterial
		{
			get => rendererComponent.sharedMaterials[materialIndex];
			set => rendererComponent.sharedMaterials[materialIndex] = value;
		}

		public void ForceReInitialize()
		{
			Initialize();
		}

		void Start()
		{
			Initialize();
		}

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			Initialize();
		}
#endif

		void Initialize()
		{
			GetRenderer();

			SaveInitialRendererSettings();
		}

		void GetRenderer()
		{
			if(autoSetRendererWhenNull && rendererComponent == null)
				rendererComponent = GetComponent<Renderer>();
		}

		void SaveInitialRendererSettings()
		{
			SaveBaseEmissiveColor();
			SaveInitialMaterial();
		}

		void SaveBaseEmissiveColor()
		{
			if(rendererComponent == null)
				return;

			baseEmissiveColor = SharedMaterial.GetColor(emissionColor_ShaderPropertyName);
		}

		void SaveInitialMaterial()
		{
			if(rendererComponent == null)
				return;

			if(EmissionPercent == 0.0f)
				initialMaterial = SharedMaterial;
		}

		void OnEmissionPercentChange()
		{
			if(rendererComponent == null)
				return;

			if(forcedOtherMaterial && EmissionPercent != 0.0f)
			{
				SharedMaterial = forcedOtherMaterial;
			}
			else
			{
				SharedMaterial = initialMaterial;
			}
		}
	}
}
