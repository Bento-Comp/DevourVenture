using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniSpawn
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteAlways]
	[AddComponentMenu("UniSpawn/InstanceList_Base")]
	public abstract class InstanceList_Base : MonoBehaviour
	{
		public System.Action onUpdateList;

		public abstract int Count
		{
			get;
		}

		protected void NotifyUpdateListCompleted()
		{
			onUpdateList?.Invoke();
		}

		protected abstract void UpdateList();
	}
}
