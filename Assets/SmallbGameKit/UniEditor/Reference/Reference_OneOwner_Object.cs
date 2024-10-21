using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniEditor
{
	public class Reference_OneOwner_Object : ScriptableObject
	{
		[HideInInspector]
		[SerializeField]
		bool ownResource;
		
		[HideInInspector]
		[SerializeField]
		Object reference;

		//Hack_Sev in order to work with additive scene without the annoying cross reference issue pop up each save
		// don't serialize the owner which force mesh update event if not needed (Scene reload for example or script compilation)
		//[HideInInspector]
		//[SerializeField]
		Object owner; 
		
		public Object Reference
		{
			get
			{
				return reference;
			}
			
			set
			{
				reference = value;
			}
		}
		
		public Object Owner
		{
			get
			{
				return owner;
			}
		}
		
		public void Create(Object a_rReference, Object a_rOwner, bool a_bOwnResource)
		{
			reference = a_rReference;
			owner = a_rOwner;
			ownResource = a_bOwnResource;
		}
		
		void OnDestroy()
		{
			if(ownResource)
			{
				if(reference != null)
				{
					DestroyImmediate(reference);
				}
			}
		}
	}
}
