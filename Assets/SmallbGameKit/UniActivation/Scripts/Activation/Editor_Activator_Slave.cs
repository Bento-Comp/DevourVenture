using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/Editor_Activator_Slave")]
	public class Editor_Activator_Slave : MonoBehaviour
	{
		Activator activator_slave;

		public Activator activator_master;

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			if(activator_slave == null)
			{
				activator_slave = GetComponent<Activator>();
				if(activator_slave == null)
					return;
			}

			if(activator_master == null)
				return;

			activator_slave.SelectedIndex = activator_master.SelectedIndex;
		}
#endif
	}
}