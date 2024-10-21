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
	[AddComponentMenu("UniSkin/SkinUserBase")]
	public abstract class SkinUserBase : MonoBehaviour
	{
		[SerializeField]
		SkinUserBase parentSkinUser;

		[SerializeField]
		List<Skin> skins =  new List<Skin>();

		// Applicators

		List<SkinItemApplicatorBase> applicators = new List<SkinItemApplicatorBase>();

		List<SkinItemApplicatorBase> applicatorsToUnregister = new List<SkinItemApplicatorBase>();
		List<SkinItemApplicatorBase> applicatorsToRegister = new List<SkinItemApplicatorBase>();

		bool loopingThroughApplicators;

		// Skin User Children
		List<SkinUserBase> skinUserChildren = new List<SkinUserBase>();

		List<SkinUserBase> skinUserChildrenToUnregister = new List<SkinUserBase>();
		List<SkinUserBase> skinUserChildrenToRegister = new List<SkinUserBase>();

		bool loopingThroughSkinUserChildren;

		public SkinUserBase ParentSkinUser
		{
			get
			{
				return parentSkinUser;
			}

			set
			{
				if(parentSkinUser == value)
					return;

				SetParentSkinUser(value);
			}
		}

		public List<Skin> Skins
		{
			get
			{
				return skins;
			}

			set
			{
				skins = value;
			}
		}

		public int SkinLayerCount
		{
			get
			{
				return skins.Count;
			}
		}

		public void CopySkinsTo(SkinUserBase skinUser)
		{
			for(int i = 0; i < skins.Count && i < skinUser.skins.Count; ++i)
			{
				Skin skin = skins[i];

				if(skin == null)
					continue;

				skinUser.SetSkin(i, skin);
			}
		}

		public Skin GetSkin(int skinIndex)
		{
			Skin skin = Skins[skinIndex];
			//Debug.Log("SkinUserBase : GetSkin : skinIndex = " + skinIndex + " | skin = " + skin);
			return skin;	
		}

		public void SetSkin(int skinIndex, Skin skin)
		{
			Skin lastSkin = Skins[skinIndex];
			//Debug.Log("SkinUserBase : SetSkin : skinIndex = " + skinIndex + " | skin = " + skin + " | lastSkin = " + lastSkin);
			Skins[skinIndex] = skin;

			if(lastSkin != skin)
			{
				OnSkinChange(skinIndex);
			}
		}

		public virtual SkinItemType GetSkinItem<SkinItemType>(string skinItemName) where SkinItemType : SkinItemBase
		{
			SkinItemType skinItem = null;
			for(int i = skins.Count - 1; i >= 0; --i)
			{
				if(TryGetSkinItem<SkinItemType>(skinItemName, skins[i], i, out skinItem))
				{
					return skinItem;
				}
			}

			if(skinItemName == "TrailColor")
			{
				bool set = true;
				if(set == false)
					return null;
			}

			if(skinItem == null && parentSkinUser != null)
			{
				return parentSkinUser.GetSkinItem<SkinItemType>(skinItemName);
			}

			return null;
		}

		public void Register(SkinItemApplicatorBase applicator)
		{
			if(loopingThroughApplicators)
			{
				applicatorsToRegister.Add(applicator);
				return;
			}

			applicators.Add(applicator);
		}

		public void Unregister(SkinItemApplicatorBase applicator)
		{
			if(loopingThroughApplicators)
			{
				applicatorsToUnregister.Add(applicator);
				return;
			}

			applicators.Remove(applicator);
		}

		protected virtual bool TryGetSkinItem<SkinItemType>(string skinItemName, Skin skin, int skinIndex, out SkinItemType skinItem) where SkinItemType : SkinItemBase
		{
			skinItem = null;

			if(skin == null)
				return false;

			skinItem = skin.GetSkinItem<SkinItemType>(skinItemName);

			return skinItem != null;
		}

		protected virtual void OnSkinChange(int skinIndex)
		{
			loopingThroughApplicators = true;
			foreach(SkinItemApplicatorBase applicator in applicators)
				applicator.NotifySkinChange();
			loopingThroughApplicators = false;
			DelayedRegisterApplicators();

			loopingThroughSkinUserChildren = true;
			foreach(SkinUserBase child in skinUserChildren)
			{
				child.OnSkinChange(skinIndex);
			}
			loopingThroughSkinUserChildren = false;
			DelayedRegisterSkinUserChildren();
		}

		protected virtual void Awake()
		{
			SetParentSkinUser(parentSkinUser);
		}

		protected virtual void OnDestroy()
		{
			SetParentSkinUser(null);
		}

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying == false)
			{
				NotifySkinChange();
			}
		}
		#endif

		void RegisterChild(SkinUserBase skinUserChild)
		{
			if(loopingThroughSkinUserChildren)
			{
				skinUserChildrenToRegister.Add(skinUserChild);
				return;
			}

			skinUserChildren.Add(skinUserChild);
		}

		void UnregisterChild(SkinUserBase skinUserChild)
		{
			if(loopingThroughSkinUserChildren)
			{
				skinUserChildrenToUnregister.Add(skinUserChild);
				return;
			}

			skinUserChildren.Remove(skinUserChild);
		}

		void SetParentSkinUser(SkinUserBase newParentSkinUser)
		{
			if(parentSkinUser != null)
			{
				parentSkinUser.UnregisterChild(this);
			}

			parentSkinUser = newParentSkinUser;

			if(parentSkinUser != null)
			{
				parentSkinUser.RegisterChild(this);
			}
		}

		void NotifySkinChange()
		{
			#if UNITY_EDITOR
			for(int i = applicators.Count - 1; i >= 0; --i)
			{
				if(applicators[i] == null)
					applicators.RemoveAt(i);
			}
			#endif

			foreach(SkinItemApplicatorBase applicator in applicators)
			{
				applicator.NotifySkinChange();
			}
		}

		void DelayedRegisterApplicators()
		{
			foreach(SkinItemApplicatorBase applicator in applicatorsToRegister)
			{
				applicators.Add(applicator);
			}
			applicatorsToRegister.Clear();

			foreach(SkinItemApplicatorBase applicator in applicatorsToUnregister)
			{
				applicators.Remove(applicator);
			}
			applicatorsToUnregister.Clear();
		}

		void DelayedRegisterSkinUserChildren()
		{
			foreach(SkinUserBase children in skinUserChildrenToRegister)
			{
				skinUserChildren.Add(children);
			}
			skinUserChildrenToRegister.Clear();

			foreach(SkinUserBase children in skinUserChildrenToUnregister)
			{
				skinUserChildren.Remove(children);
			}
			skinUserChildrenToUnregister.Clear();
		}
	}
}
