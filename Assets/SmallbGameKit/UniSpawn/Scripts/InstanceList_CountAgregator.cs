using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniSpawn
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteAlways]
	[AddComponentMenu("UniSpawn/InstanceList_CountAgregator")]
	public class InstanceList_CountAgregator : MonoBehaviour
	{
		public InstanceList_Base instanceList;

		public List<InstanceList_Base> instanceLists;

		int TotalCount
		{
			get
			{
				int totalCount = 0;

				foreach(InstanceList_Base instanceList in instanceLists)
				{
					if(instanceList == null)
						continue;

					totalCount += instanceList.Count;
				}

				return totalCount;
			}
		}


		void OnEnable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			UpdateCount();
			foreach(InstanceList_Base instanceList in instanceLists)
			{
				if(instanceList == null)
					continue;

				instanceList.onUpdateList += OnUpdateList;
			}
		}

		void OnDisable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			foreach(InstanceList_Base instanceList in instanceLists)
			{
				if(instanceList == null)
					continue;

				instanceList.onUpdateList -= OnUpdateList;
			}
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			if(instanceList == null)
				return;

			UpdateCount();
		}
		#endif

		void OnUpdateList()
		{
			UpdateCount();
		}

		void UpdateCount()
		{
			ISetCountInstanceList setCountInstanceList = instanceList as ISetCountInstanceList;

			if(setCountInstanceList == null)
				return;

			setCountInstanceList.SetCount(TotalCount);
		}
	}
}
