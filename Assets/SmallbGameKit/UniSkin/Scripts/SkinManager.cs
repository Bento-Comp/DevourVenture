using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniSkin
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniSkin/SkinManager")]
	public class SkinManager : SkinUserBase
	{
		public static System.Action<int> onSkinChange;

		public bool debug_updateSkinInRuntime = true;

		// Use this with caution:
		// useful when you need to switch
		// between multiple instances
		// (ex: for Game Mode or AB tests)
		public bool canSwitchInstanceAtRuntime;

		public static SkinManager Instance { get; private set; }

		void OnEnable()
		{
			if(Instance == null || canSwitchInstanceAtRuntime)
			{
				Instance = this;
			}
			else
			{
#if UNITY_EDITOR
				if(Application.isPlaying == false)
					return;
#endif

				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}

		void OnDisable()
		{
			if(Instance == this)
			{
				Instance = null;
			}
		}

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying == false)
			{
				Instance = this;
			}
		}
#endif

		protected override void OnSkinChange(int skinIndex)
		{
			base.OnSkinChange(skinIndex);

			onSkinChange?.Invoke(skinIndex);
		}
	}
}
