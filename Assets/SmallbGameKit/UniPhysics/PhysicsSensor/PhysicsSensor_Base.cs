using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensor_Base")]
	public abstract class PhysicsSensor_Base : MonoBehaviour
	{
		[SerializeField]
		string sensorLayerName = "default";

		PhysicsSensorManager manager;

		public string SensorLayerName => sensorLayerName;

		protected virtual void Initialise()
		{
		}

		protected abstract void Register(PhysicsSensorManager manager);

		protected abstract void Unregister(PhysicsSensorManager manager);

		protected virtual void OnEnable()
        {
			manager = PhysicsSensorManager.Instance;

			Register(manager);

			Initialise();
		}

		protected virtual void OnDisable()
		{
			if(manager != null)
				Unregister(manager);
		}
    }
}