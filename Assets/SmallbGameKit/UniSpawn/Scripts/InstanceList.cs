using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniSpawn
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteAlways]
	[AddComponentMenu("UniSpawn/InstanceList")]
	public abstract class InstanceList<InstanceType> : InstanceList_Base where InstanceType : Component
	{
		[Header("Instance List")]
		public bool updateInEditMode;

		public InstanceType prefab;

		public bool reverseSiblingOrder;

		public Transform instancesRootTransform;

		[SerializeField]
        List<InstanceType> instances = new List<InstanceType>();

		public List<InstanceType> Instances
		{
			get
			{
				return instances;
			}
		}

		protected virtual void OnSetupInstance(InstanceType instance, int instanceIndex)
		{
		}

		protected override void  UpdateList()
        {
            int index = 0;
            for(int i = 0; i < Count; ++i)
            {
                if(index >= instances.Count)
                {
                    instances.Add(null);
                }

                InstanceType instance = instances[index];

                if(instance == null)
                {
                    instance = CreateInstance();
                    instances[index] = instance;
                }

                SetupInstance(instance, index);

                ++index;
            }

            while(instances.Count > index)
            {
                int indexToRemove = instances.Count - 1;

                InstanceType instanceToRemove = instances[indexToRemove];

                if(instanceToRemove != null)
                    DestroyImmediate(instanceToRemove.gameObject);

                instances.RemoveAt(indexToRemove);
            }

			NotifyUpdateListCompleted();
        }

		#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			if(updateInEditMode == false)
				return;

			if(prefab == null)
				return;

			if(instancesRootTransform == null)
				return;

			UpdateList();
		}
		#endif

        InstanceType CreateInstance()
        {
			InstanceType instance;
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				instance = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as InstanceType;
				instance.transform.SetParent(instancesRootTransform, false);
			}
			else
			#endif
			{
				instance = Instantiate(prefab, instancesRootTransform, false);
			}

            return instance;
        }

        void SetupInstance(InstanceType instance, int instanceIndex)
        {
            instance.name = prefab.name + " (" + (instanceIndex + 1) + ")";

			if(reverseSiblingOrder)
			{
				instance.transform.SetAsFirstSibling();
			}
			else
			{
				instance.transform.SetAsLastSibling();
			}

			OnSetupInstance(instance, instanceIndex);
        }
	}
}
