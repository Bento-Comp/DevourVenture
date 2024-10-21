using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniPhysics
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("TemplateFolder/PhysicsSensorManager")]
	public class PhysicsSensorManager : UniSingleton.Singleton<PhysicsSensorManager>
	{
		[Header("Overlap Layers")]
		public List<PhysicsSensorLayer_Base> sensorLayers;

		Dictionary<string, PhysicsSensorLayer_Base> sensorLayerByName = new Dictionary<string, PhysicsSensorLayer_Base>();

		public SensorLayerType GetSensorLayer<SensorLayerType>(string sensorLayerName) where SensorLayerType : PhysicsSensorLayer_Base
		{
			return sensorLayerByName[sensorLayerName] as SensorLayerType;
		}

        void Awake()
        {
			FillDictionary();    
        }

		void FillDictionary()
		{
			foreach(PhysicsSensorLayer_Base sensorLayer in sensorLayers)
			{
				sensorLayerByName.Add(sensorLayer.SensorLayerName, sensorLayer);
			}
		}
    }
}