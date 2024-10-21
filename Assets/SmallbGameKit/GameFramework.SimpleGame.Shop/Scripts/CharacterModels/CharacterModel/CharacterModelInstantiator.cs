using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniSkin;

namespace GameFramework.SimpleGame
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/CharacterModelInstantiator")]
	public class CharacterModelInstantiator : MonoBehaviour
	{
		public System.Action<CharacterModel> afterCharacterModelSetup;

		public bool updateInEditor;

		[Header("Model")]

		[SerializeField]
		CharacterModel  characterModelPrefab;

		[Header("Settings")]

		public SkinUserBase skinUser;

		public bool overrideLayer;
		public string layerName;

		public bool overrideRuntimeAnimatorController;
		public RuntimeAnimatorController runtimeAnimatorController;

		public bool overrideAvatar;
		public Avatar avatar;

		[Header("Auto-Filled")]
		[SerializeField]
		CharacterModel  currentInstancePrefab;

		[SerializeField]
		CharacterModel  characterModelInstance;

		public CharacterModel CharacterModelInstance => characterModelInstance;

		public CharacterModel  CharacterModelPrefab
		{
			set
			{
				if(characterModelPrefab != value)
				{
					characterModelPrefab = value;
					UpdateModelInstance();
				}
			}

			get
			{
				return characterModelPrefab;
			}
		}

		#if UNITY_EDITOR
			bool Editor_CantUpdate => updateInEditor == false && Application.isPlaying == false;
		#endif


#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			if(characterModelPrefab == null)
				return;

			UpdateModelInstance();
		}
#endif

		void UpdateModelInstance()
		{
			#if UNITY_EDITOR
				if(Editor_CantUpdate)
					return;
			#endif

			if(currentInstancePrefab != characterModelPrefab
				&& characterModelInstance != null)
			{
				DestroyImmediate(characterModelInstance.gameObject);
				characterModelInstance = null;
			}

			if(characterModelInstance == null)
			{
				characterModelInstance = InstantiatePrefab<CharacterModel>(characterModelPrefab, transform);
				currentInstancePrefab = characterModelPrefab;
			}

			Setup(characterModelInstance);

			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				UnityEditor.EditorUtility.SetDirty(this);
			}
			#endif
		}

		void Setup(CharacterModel  model)
		{
			if(model.modelSkinUser != null)
				model.modelSkinUser.ParentSkinUser = skinUser;

			if(overrideLayer)
			{
				CharacterModelsUtilities.SetLayer_AllChildren(model.gameObject, layerName);
			}

			if(overrideRuntimeAnimatorController)
			{
				model.animator.runtimeAnimatorController = runtimeAnimatorController;
			}

			if(overrideAvatar)
			{
				model.animator.avatar = avatar;
			}

			afterCharacterModelSetup?.Invoke(model);
		}

		TInstance InstantiatePrefab<TInstance>(TInstance prefab, Transform parent) where TInstance : Object
		{
			TInstance instance;

			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent) as TInstance;
			}
			else
			#endif
			{
				instance = Instantiate<TInstance>(prefab, parent, false);
			}

			return instance;
		}
	}
}