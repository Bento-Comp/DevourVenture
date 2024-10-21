using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniMaterial
{
	[DefaultExecutionOrder(1)]
	[ExecuteAlways()]
	[AddComponentMenu("UniMaterial/EmissiveIntensityController")]
	public class EmissiveIntensityController : MonoBehaviour 
	{
		class HookBatch
		{
			public MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

			public List<EmissiveIntensityHook> hooks = new List<EmissiveIntensityHook>();

			public Color baseEmissiveColor;

			public string emissionColor_ShaderPropertyName;

			public void ApplyIntensity(Color additiveColor, float additiveColorIntensity, float baseColorIntensity, float emissionPercent)
			{
				Color color = Color.LerpUnclamped(baseEmissiveColor,
					baseEmissiveColor * baseColorIntensity + additiveColor * additiveColorIntensity,
					emissionPercent);

				foreach(EmissiveIntensityHook hook in hooks)
				{
					if(hook.rendererComponent != null)
					{
						hook.rendererComponent.GetPropertyBlock(materialPropertyBlock, hook.materialIndex);
						materialPropertyBlock.SetColor(emissionColor_ShaderPropertyName, color);

						hook.EmissionPercent = emissionPercent;
						hook.rendererComponent.SetPropertyBlock(materialPropertyBlock, hook.materialIndex);
					}
				}
			}

			public static string GetDictionnaryKey(EmissiveIntensityHook hook)
			{
				return hook.emissionColor_ShaderPropertyName + hook.BaseEmissiveColor;
			}
		}

		public Transform hooksRoot;

		public float emissionPercent = 0.0f;

		public Color additiveColor = Color.white;

		public float additiveColorIntensity = 1.0f;

		public float baseColorIntensity = 1.0f;

		public bool filterHooksByTags;

		public List<string> hookTags;

		float lastEmissionPercent;

		Color lastAdditiveColor;

		float lastBaseColorIntensity;

		float lastAdditiveColorIntensity;

		List<EmissiveIntensityHook> hooks;

		Dictionary<string, HookBatch> hookBatchDictionary = new Dictionary<string, HookBatch>();

		List<HookBatch> hookBatches = new List<HookBatch>();

		public float AdditiveColorIntensity_RuntimeMultiplicator { get; set; } = 1.0f;

		float AdditiveEmissiveIntensity => additiveColorIntensity * AdditiveColorIntensity_RuntimeMultiplicator;

		public void ForceUpdateHooks()
		{
			GetHooks();
		}

		void Start()
		{
			GetHooks();
		}

		void LateUpdate()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				GetHooks();
			}
			#endif

			float intensity = AdditiveEmissiveIntensity; 
			if(intensity != lastAdditiveColorIntensity
				|| lastAdditiveColor != additiveColor
				|| lastBaseColorIntensity != baseColorIntensity
				|| lastEmissionPercent != emissionPercent)
			{
				ApplyIntensity(intensity);
			}
		}

		void ApplyIntensity(float intensity)
		{
			foreach(HookBatch batch in hookBatches)
			{
				batch.ApplyIntensity(additiveColor, intensity, baseColorIntensity, emissionPercent);
			}

			lastEmissionPercent = emissionPercent;
			lastAdditiveColor = additiveColor;
			lastBaseColorIntensity = baseColorIntensity;
			lastAdditiveColorIntensity = intensity;
		}

		void GetHooks()
		{
			if(hooksRoot == null)
				return;

			hooks = new List<EmissiveIntensityHook>(hooksRoot.GetComponentsInChildren<EmissiveIntensityHook>(true));
			if(filterHooksByTags)
			{
				List<EmissiveIntensityHook> hooksToFilter = hooks;
				hooks = new List<EmissiveIntensityHook>();
				foreach(EmissiveIntensityHook hook in hooksToFilter)
				{
					if(hookTags.Contains(hook.hookTag))
					{
						hooks.Add(hook);
					}
				}
			}
			FillDictionaryAndList();
		}

		void FillDictionaryAndList()
		{
			hookBatchDictionary.Clear();

			// Fill dictionary and list
			foreach(EmissiveIntensityHook hook in hooks)
			{
				HookBatch batch;
				string batchKey = HookBatch.GetDictionnaryKey(hook);
				if(hookBatchDictionary.TryGetValue(batchKey, out batch) == false)
				{
					batch = new HookBatch();

					batch.baseEmissiveColor = hook.BaseEmissiveColor;

					batch.emissionColor_ShaderPropertyName = hook.emissionColor_ShaderPropertyName;

					hookBatchDictionary.Add(batchKey, batch);

					hookBatches.Add(batch);
				}

				batch.hooks.Add(hook);
			}
		}
	}
}
