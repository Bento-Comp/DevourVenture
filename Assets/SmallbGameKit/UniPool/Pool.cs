using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniPool
{
	[AddComponentMenu("UniPool/Pool")]
	public class Pool<ComponentType> : PoolBase where ComponentType : PoolInstance
	{
		public ComponentType model;

		public Transform root;
		[Tooltip("Set it to true if you want correct parenting to canvas (a little more expensive)")]
		public bool forceWorldPositionStay;

		[SerializeField]
		bool useMaxActiveInstanceCount = false;

		[SerializeField]
		int maxActiveInstanceCount = 10;

		[SerializeField]
		bool recyleWhenLimitReached = false;

		[SerializeField]
		int size = 0;

		[SerializeField]
		int activeInstanceCount = 0;

		List<ComponentType> activeInstances = new List<ComponentType>();

		Stack<ComponentType> destroyed = new Stack<ComponentType>();

		public bool MaxActiveInstanceCountReached =>
			useMaxActiveInstanceCount && activeInstanceCount >= maxActiveInstanceCount;

		public int ActiveInstanceCount => activeInstanceCount;

		public int MaxActiveInstanceCount
		{
			get => maxActiveInstanceCount;
			set => maxActiveInstanceCount = value;
		}

		public ComponentType TryCreatePoolInstance()
		=> _TryCreatePoolInstance(false, Vector3.zero, Quaternion.identity);

		public ComponentType TryCreatePoolInstance(Vector3 position, Quaternion rotation)
		=> _TryCreatePoolInstance(true, position, rotation);

		public override void DestroyPoolInstance(PoolInstance instance)
		{
			if(instance.CanDestroy == false)
				return;

			ComponentType componentType = instance as ComponentType;

			base.DestroyPoolInstance(instance);
			destroyed.Push(componentType);

			if(recyleWhenLimitReached)
				activeInstances.Remove(componentType);

			UpdateActiveInstanceCount();
		}

		protected virtual void Awake()
		{
			/*
			model.CanDestroy = false;
			model.gameObject.SetActive(false);
			model.PoolInstanceIsDestroyed = true;
			*/
			CreatePool();
		}

		ComponentType _TryCreatePoolInstance(bool setPositionAndRotation, Vector3 position, Quaternion rotation)
		{
			// If we have reached the max active instance count
			// Recycle the oldest instance if we are allowed to
			// Or just give up and return null
			if(MaxActiveInstanceCountReached)
			{
				if(recyleWhenLimitReached == false)
					return null;

				if(activeInstances.Count <= 0)
					return null;

				ComponentType toRecycle = activeInstances[0];
				toRecycle.DestroyPoolInstance();
			}

			ComponentType instance;

			if(destroyed.Count > 0)
			{
				instance = destroyed.Pop();

				Transform instanceTransform = instance.transform;
				if(setPositionAndRotation)
				{
					instanceTransform.position = position;
					instanceTransform.rotation = rotation;
				}

				/*if(instance.PoolInstanceIsDestroyed == false)
				{
					Debug.LogError("Pop an active object : " + instance);
				}*/
			}
			else
			{
				if(setPositionAndRotation)
				{
					instance = InstantiatePoolInstance(position, rotation);
				}
				else
				{
					instance = InstantiatePoolInstance();
				}
				++size;
			}

			if(recyleWhenLimitReached)
				activeInstances.Add(instance);

			UpdateActiveInstanceCount();

			if(root != null)
			{
				instance.transform.SetParent(root, forceWorldPositionStay);
			}

			instance.PoolInstanceIsDestroyed = false;
			instance.gameObject.SetActive(true);

			instance.CreatePoolInstance();

			return instance;
		}

		void CreatePool()
		{
			for(int i = 0; i < size; ++i)
			{
				DestroyPoolInstance(InstantiatePoolInstance());
			}
		}

		ComponentType InstantiatePoolInstance()
		{
			ComponentType instance = Instantiate(model, transform, false) as ComponentType;
			instance.Initialize(this);

			return instance;
		}

		ComponentType InstantiatePoolInstance(Vector3 position, Quaternion rotation)
		{
			ComponentType instance = Instantiate(model, position, rotation, transform) as ComponentType;
			instance.Initialize(this);

			return instance;
		}

		void UpdateActiveInstanceCount()
		{
			activeInstanceCount = size - destroyed.Count;
		}
	}
}