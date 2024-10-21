using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/Editor_Activator_Master")]
	public class Editor_Activator_Master : MonoBehaviour
	{
		public Activator activator_slave;

		Activator activator_master;

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			if(activator_master == null)
			{
				activator_master = GetComponent<Activator>();
				if(activator_master == null)
					return;
			}

			if(activator_slave == null)
				return;

			activator_slave.SelectedIndex = activator_master.SelectedIndex;
		}
#endif
	}
}