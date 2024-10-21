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
	[AddComponentMenu("UniActivation/InstanceList_Activator")]
	public class InstanceList_Activator : InstanceList_WithCount<Activator>, IActivatorList
	{
		public int ActivatorCount => Instances.Count;

		public List<Activator> Activators => Instances;

		public void AddListChangeListener(System.Action onListChange)
		{
			onUpdateList += onListChange;
		}

		public void RemoveListChangeListener(System.Action onListChange)
		{
			onUpdateList -= onListChange;
		}
	}
}