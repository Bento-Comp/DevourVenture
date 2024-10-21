using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[DefaultExecutionOrder(-31998)]
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/Activator")]
	public class Activator : MonoBehaviour, IActivatorList
	{
		public Action<int, bool> onUpdateActivation_withParameters;
		public Action onUpdateActivation;

		[SerializeField]
		int selectedIndex;

		public List<Activator> childrenActivators = new List<Activator>();

		public List<ActivationGroup> activationGroups = new List<ActivationGroup>();

		// IActivatorList : Allow an Activator to Act as a List of one Item
		List<Activator> activatorList = null;
		public int ActivatorCount => 1;
		public List<Activator> Activators
		{
			get
			{
				if(activatorList == null || activatorList.Count <= 0)
					activatorList = new List<Activator>(){this};

				return activatorList;
			}
		}
		public void AddListChangeListener(System.Action onListChange){}
		public void RemoveListChangeListener(System.Action onListChange){}

		int lastSelectedIndex;

		public int SelectedIndex
		{
			get
			{
				return selectedIndex;
			}

			set
			{
				int clampedValue = ClampIndex(value);
				if(clampedValue != selectedIndex)
				{
					selectedIndex = clampedValue;
					UpdateActivation();
				}
			}
		}

		public int IndexCount => activationGroups.Count;

		public int LastIndex => activationGroups.Count - 1;

		public void SetFirstActiveState(int index)
		{
			selectedIndex = ClampIndex(index);
			UpdateActivation(selectedIndex, true);
		}

		public void ForceActive(int index)
		{
			if(index >= 0 && index < activationGroups.Count)
				activationGroups[index].Activate(true, null);
		}

		public void ForceReset()
		{
			for(int i = 0; i < activationGroups.Count; ++i)
			{
				activationGroups[i].Activate(false, null);
			}
			UpdateActivation();
		}

		public void ForceUpdateActivation(int selectedIndex, bool setFirstState = false)
		{
			this.selectedIndex = selectedIndex;
			UpdateActivation(selectedIndex, setFirstState);
		}

		void Awake()
		{
			if(isActiveAndEnabled == false)
				return;

			selectedIndex = ClampIndex(selectedIndex);

#if UNITY_EDITOR
			Editor_PlayMode_SaveSelectedIndex();
#endif
		}

#if UNITY_EDITOR
		void Update()
		{
			Editor_UpdateActivation();
		}

		int editor_playMode_lastSelectedIndex;
		void Editor_PlayMode_SaveSelectedIndex()
		{
			if(Application.isPlaying == false)
				return;
			
			editor_playMode_lastSelectedIndex = selectedIndex;
		}

	void Editor_UpdateActivation()
		{
			selectedIndex = ClampIndex(selectedIndex);

			if(Application.isPlaying)
			{
				if(editor_playMode_lastSelectedIndex == selectedIndex)
					return;
			}

			UpdateActivation();
		}
#endif

		int ClampIndex(int index)
		{
			return Mathf.Clamp(index, 0, activationGroups.Count - 1);
		}

		void UpdateActivation()
		{
			int selectedIndex = SelectedIndex;
			UpdateActivation(selectedIndex);
		}

		void UpdateActivation(int selectedIndex, bool setFirstState = false)
		{
#if UNITY_EDITOR
			Editor_PlayMode_SaveSelectedIndex();
#endif
			if(selectedIndex < 0 || selectedIndex >= activationGroups.Count)
			{
				selectedIndex = -1;
			}
			
			ActivationGroup selectedActivationGroup = null;
			if(selectedIndex != -1)
			{
				selectedActivationGroup = activationGroups[selectedIndex];
				selectedActivationGroup.Activate(true, childrenActivators, null, setFirstState);
			}
			
			for(int i = 0; i < activationGroups.Count; ++i)
			{
				if(i != SelectedIndex)
				{
					activationGroups[i].Activate(false, childrenActivators, selectedActivationGroup, setFirstState);
				}
			}

			onUpdateActivation_withParameters?.Invoke(selectedIndex, setFirstState);
			onUpdateActivation?.Invoke();
		}
	}
}