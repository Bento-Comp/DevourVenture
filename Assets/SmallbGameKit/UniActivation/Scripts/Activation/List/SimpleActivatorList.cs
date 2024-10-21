using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UniSpawn;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniActivation
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteAlways]
	[AddComponentMenu("UniActivation/SimpleActivatorList")]
	public class SimpleActivatorList : MonoBehaviour, IActivatorList
	{
		public List<Activator> activators = new List<Activator>();

		public int ActivatorCount => activators.Count;

		public List<Activator> Activators => activators;

		System.Action onListChange;

		#if UNITY_EDITOR
		List<Activator> activatorsCopy = new List<Activator>();
		#endif

		public void AddListChangeListener(System.Action onListChange)
		{
			this.onListChange += onListChange;
		}

		public void RemoveListChangeListener(System.Action onListChange)
		{
			this.onListChange -= onListChange;
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			CheckForChanges();
		}

		void CheckForChanges()
		{
			if(AreEquals(activators, activatorsCopy) == false)
			{
				activatorsCopy.Clear();
				activatorsCopy.AddRange(activators);

				onListChange?.Invoke();
			}
		}

		static bool AreEquals(List<Activator> activatorsA, List<Activator> activatorsB)
		{
			if(activatorsA.Count != activatorsB.Count)
				return false;

			for(int i = 0; i < activatorsA.Count; ++i)
			{
				if(activatorsB[i] != activatorsA[i])
					return false;
			}

			return true;
		}
		#endif
	}
}