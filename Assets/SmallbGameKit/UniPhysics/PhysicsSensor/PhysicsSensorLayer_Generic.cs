using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensorLayer_Base")]
	public abstract class PhysicsSensorLayer_Generic<SensorType> : PhysicsSensorLayer_Base where SensorType : PhysicsSensor_Base
	{
		[SerializeField]
		public LayerMask layerMask;

		[SerializeField]
		protected QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;

		[Header("Optimisation")]

		[SerializeField]
		float maxSensorUpdateBySecond = 10.0f;

		[SerializeField]
		protected int maxColliderBySensorUpdate = 100;

		List<SensorType> sensors = new List<SensorType>();

		List<SensorType> delayedSensorToRemove = new List<SensorType>();

		bool delayedRemove;

		float sensorUpdateRemainder;

		int currentSensorIndex;

		protected int LayerMask { get; private set; }

		public void Add(SensorType sensor)
		{
			sensors.Add(sensor);
		}

		public void Remove(SensorType sensor)
		{
			if(delayedRemove)
			{
				delayedSensorToRemove.Add(sensor);
				return;
			}
			sensors.Remove(sensor);
		}

		protected virtual void Initialise()
		{
		}

		protected abstract void UpdateSensor(SensorType sensor);

		void Awake()
        {
			Initialise();
			LayerMask = layerMask.value;
		}

        void FixedUpdate()
        {
			int overlapperCount = sensors.Count;

			float overlapToProcessThisFrame_Float = maxSensorUpdateBySecond * Time.deltaTime + sensorUpdateRemainder;
			int overlapToProcessThisFrame_Int = (int)overlapToProcessThisFrame_Float;
	
			int loopCount;
			if(overlapToProcessThisFrame_Int >= overlapperCount)
			{
				loopCount = overlapperCount;
				sensorUpdateRemainder = 0;
			}
			else
			{
				loopCount = overlapToProcessThisFrame_Int;
				sensorUpdateRemainder = overlapToProcessThisFrame_Float - overlapToProcessThisFrame_Int;
			}

			delayedRemove = true;
			for(int i = 0; i < loopCount; ++i)
			{
				currentSensorIndex = currentSensorIndex % overlapperCount;

				UpdateSensor(sensors[currentSensorIndex]);

				++currentSensorIndex;
			}
			delayedRemove = false;

			foreach(SensorType sensor in delayedSensorToRemove)
			{
				Remove(sensor);
			}
			delayedSensorToRemove.Clear();
		}
    }
}