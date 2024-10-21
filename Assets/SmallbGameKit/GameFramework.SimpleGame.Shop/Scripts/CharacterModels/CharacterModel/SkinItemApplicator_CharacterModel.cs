using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniSkin;

namespace GameFramework.SimpleGame
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/SkinItemApplicator_CharacterModel")]
	public class SkinItemApplicator_CharacterModel : SkinItemApplicatorBase
	{
		public CharacterModelInstantiator modelInstantiator;

		public bool onlyUpdateOnCover = false;

		protected override void OnSkinChange()
		{
			SkinItem_CharacterModel skinItem = GetSkinItem<SkinItem_CharacterModel>(skinItemName);

			#if UNITY_EDITOR
			if(Application.isPlaying == false && skinItem == null)
				return;

			if(Application.isPlaying)
			{
				if(skinItem == null)
				{
					//Debug.LogError("Skin not found : skinItemName = " + skinItemName + " | this : " + this);
					return;
				}
			}
			#endif

			OnModelChange(skinItem.GetModel());
		}

		void OnModelChange(CharacterModel  model)
		{
			if(modelInstantiator == null)
				return;

			if(Application.isPlaying)
			{
				if(onlyUpdateOnCover && Game.Instance.IsCover == false)
					return;
			}

			modelInstantiator.CharacterModelPrefab = model;
		}
	}
}