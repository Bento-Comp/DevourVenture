using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniPhysics
{
	[AddComponentMenu("UniPhysics/PhysicsSensorLayer_Base")]
	public abstract class PhysicsSensorLayer_Base : MonoBehaviour
	{
		[Header("Identifier")]
		[SerializeField]
		string sensorLayerName = "default";

		public string SensorLayerName => sensorLayerName;
    }
}