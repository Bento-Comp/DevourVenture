using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{

	
	[ExecuteAlways()]
	[DefaultExecutionOrder(-31997)]
	[AddComponentMenu("UniActivation/NamedActivation_Activator")]
	public class NamedActivation_Activator : NamedActivation_ActivatorBase
	{
		[System.Serializable]
        public class ActivationNameIndex
        {
			public string activationName;
			public int activationIndex;
        }

		[SerializeField]
		List<ActivationNameIndex> activationNames = new List<ActivationNameIndex>();

		[Header("Editor")]
		public bool editor_autoFill = true;

		protected override int ComputeActivationIndex()
		{
			int selectedIndex = activator.SelectedIndex;

			foreach(ActivationNameIndex item in activationNames)
            {
                if(register.IsActive(item.activationName))
                {
					selectedIndex = item.activationIndex;
					break;
                }
            }

			return selectedIndex;
		}

		#if UNITY_EDITOR
		protected override void OnEditorUpdate_EditMode()
		{
			if(editor_autoFill)
			{
				AutoFill();
			}
		}

		void AutoFill()
		{
			// Fill activator
			if(activator == null)
			{
				activator = gameObject.GetComponent<Activator>();
				if(activator == null)
					activator = gameObject.AddComponent<Activator>();
			}
			List<ActivationGroup> activationGroups = activator.activationGroups;

			activationGroups.Clear();

			foreach(Transform child in transform)
			{
				ActivationGroup activationGroup = new ActivationGroup();

				activationGroup.gameObjects = new GameObject[]{child.gameObject};

				activationGroups.Add(activationGroup);
			}

			// Fill this component
			activationNames.Clear();

			foreach(Transform child in transform)
			{
				ActivationNameIndex activationNamedIndex = new ActivationNameIndex();

				activationNamedIndex.activationIndex = activationNames.Count;
				activationNamedIndex.activationName = child.name;

				activationNames.Add(activationNamedIndex);
			}
		}
		#endif
	}
}