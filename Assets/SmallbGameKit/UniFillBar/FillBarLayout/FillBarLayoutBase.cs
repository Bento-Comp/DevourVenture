using UnityEngine;

namespace UniFillBar
{
	public abstract class FillBarLayoutBase : MonoBehaviour
	{
		public FillBar fillBar;

		public abstract void UpdateLayout();

		void Awake()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif
			fillBar.onUpdateFillContainer += OnUpdateFillContainer;
		}

		void OnDestroy()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif
			fillBar.onUpdateFillContainer -= OnUpdateFillContainer;
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			UpdateLayout();
		}
		#endif

		void OnUpdateFillContainer()
		{
			UpdateLayout();
		}
	}
}