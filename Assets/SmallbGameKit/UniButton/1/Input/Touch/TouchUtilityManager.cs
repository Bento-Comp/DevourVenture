using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniButton
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniButton/TouchUtilityManager")]
	public class TouchUtilityManager : MonoBehaviour
	{
		public bool editor_override_dpi = true;
		public float editor_dpi_override = 221.0f / 0.81f;

		static TouchUtilityManager instance;
		
		public static TouchUtilityManager Instance
		{
			get
			{
				return instance;
			}
		}
		
		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying == false)
			{
				instance = this;
			}
		}
		#endif
	}
}