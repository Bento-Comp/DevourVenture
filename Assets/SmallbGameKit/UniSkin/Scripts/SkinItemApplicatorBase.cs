using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniSkin
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinItemApplicatorBase")]
	public class SkinItemApplicatorBase : MonoBehaviour
	{
		public bool autofillSkinUser = true;
		
		public SkinUserBase skinUser;

		public string skinItemName;

		bool awaken;

		bool started;

		bool registered;

		public void NotifySkinChange()
		{
			//Debug.Log("SkinItemApplicatorBase : NotifySkinChange : " + this);
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(SkinUser == null)
					return;
			}
			#endif

			OnSkinChange();
		}

		public SkinItemType GetSkinItem<SkinItemType>(string skinItemName) where SkinItemType : SkinItemBase
		{
			return SkinUser.GetSkinItem<SkinItemType>(skinItemName);
		}

		public SkinUserBase SkinUser
		{
			get
			{
				if(skinUser != null)
				{
					return skinUser;
				}

				return SkinManager.Instance;
			}
		}

		protected virtual void OnSkinChange()
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnAwakeEnd()
		{
		}

		protected virtual void OnStart()
		{
		}

		protected virtual void OnStartEnd()
		{
		}

		void Awake()
		{			
			if(awaken)
				return;

			OnAwake();
			awaken = true;
		}

		void Start()
		{
			if(started)
				return;
		
			if(Application.isPlaying)
			{
				Register();
			}

			AutoFillSkinUser();

			OnStart();
			started = true;

			NotifySkinChange();
		}

		void OnDestroy()
		{
			if(started)
			{
				OnStartEnd();
				started = false;
			}

			if(awaken)
			{
				Unregister();

				OnAwakeEnd();
				awaken = false;
			}
		}

		#if UNITY_EDITOR || debugSkin
		void Update()
		{
			if(Application.isPlaying == false)
			{
				AutoFillSkinUser();
				if(registered == false)
				{
					Register();
				}
				NotifySkinChange();
			}
			else
			{
				if(SkinUser != null && SkinManager.Instance.debug_updateSkinInRuntime)
				{
					NotifySkinChange();
				}
			}
		}
		#endif

		void AutoFillSkinUser()
		{
			if(autofillSkinUser == false)
				return;
			
			if(skinUser == null)
			{
				SkinUserBase newSkinUser = GetComponent<SkinUserBase>();

				if(newSkinUser != null && registered)
				{
					Unregister();
					skinUser = newSkinUser;
					Register();
				}
			}
		}

		void Register()
		{
			if(registered)
				return;

			#if UNITY_EDITOR
			if(Application.isPlaying == false && SkinUser == null)
				return;
			#endif

			SkinUser.Register(this);
			registered = true;
		}

		void Unregister()
		{
			if(registered == false)
				return;

			#if UNITY_EDITOR
			if(Application.isPlaying == false && SkinUser == null)
				return;
			#endif

			if(SkinUser != null)
				SkinUser.Unregister(this);

			registered = false;
		}
	}
}
