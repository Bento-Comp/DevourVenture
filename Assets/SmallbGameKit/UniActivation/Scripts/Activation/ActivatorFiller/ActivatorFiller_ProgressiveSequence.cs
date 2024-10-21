using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/ActivatorFiller_ProgressiveSequence")]
	public class ActivatorFiller_ProgressiveSequence : MonoBehaviour
	{
		public Activator activator;

		public List<GameObject> activatorListGameObjects;

		List<IActivatorList> activatorLists;

		void OnEnable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			LazyGetActivatorList();

			foreach(IActivatorList activatorList in activatorLists)
				activatorList.AddListChangeListener(OnListChange);
		}

		void OnDisable()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
				return;
			#endif

			foreach(IActivatorList activatorList in activatorLists)
				activatorList.RemoveListChangeListener(OnListChange);
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			if(activator == null)
				return;

			LazyGetActivatorList();

			Fill();
		}
		#endif

		void OnListChange()
		{
			Fill();
		}

		void Fill()
		{
			// Set Children Activators
			List<int> activatorListCounts = new List<int>();
			int maxActivatorListCount = -1;
			List<Activator> childrenActivators = activator.childrenActivators;
			childrenActivators.Clear();
			foreach(IActivatorList activatorList in activatorLists)
			{
				List<Activator> activators = activatorList.Activators;
				childrenActivators.AddRange(activators);

				int activatorListCount = activatorList.ActivatorCount;

				activatorListCounts.Add(activatorListCount);

				if(activatorListCount > maxActivatorListCount)
					maxActivatorListCount = activatorListCount;
			}
			int childrenActivatorCount = childrenActivators.Count;

			// Set activation groups with children activators indices
			List<ActivationGroup> activationGroups = activator.activationGroups;
			activationGroups.Clear();
			for(int i = 0; i < maxActivatorListCount + 1; ++i)
			{
				bool lastActivationGroup = i >= maxActivatorListCount;

				ActivationGroup activationGroup = new ActivationGroup();

				int[] childrenIndices = new int[childrenActivatorCount];
				int childrenIndex = 0;
				foreach(int activatorListCount in activatorListCounts)
				{
					for(int k = 0; k < activatorListCount; ++k)
					{
						childrenIndices[childrenIndex] = lastActivationGroup ? 2 :( (k<i) ? 1 : 0 );
						++childrenIndex;
					}
				}
				activationGroup.childrenIndices = childrenIndices;

				activationGroups.Add(activationGroup);
			}

			activator.activationGroups = activationGroups;			
		}

		void LazyGetActivatorList()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(activatorLists != null)
				{
					activatorLists.Clear();
				}
				else
				{
					activatorLists = new List<IActivatorList>();
				}
			}
			else
			#endif
			{
				if(activatorLists != null)
				return;

				activatorLists = new List<IActivatorList>();
			}

			foreach(GameObject activatorListGameObject in activatorListGameObjects)
			{
				IActivatorList[] activatorListArray = activatorListGameObject == null ? null :
					activatorListGameObject.GetComponents<IActivatorList>();

				foreach(IActivatorList activatorList in activatorListArray)
				{
					if(activatorList == (IActivatorList)activator)
						continue;

					activatorLists.Add(activatorList);
				}
			}
		}
	}
}
