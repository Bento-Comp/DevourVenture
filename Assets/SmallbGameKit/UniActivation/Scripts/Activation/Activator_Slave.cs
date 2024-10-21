using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/Activator_Slave")]
	public class Activator_Slave : MonoBehaviour
	{
		public Activator activator_master;

		Activator activator_slave;

		void Awake()
		{
			if(Application.isPlaying == false)
				return;

			LazyGetActivator();

			activator_master.onUpdateActivation_withParameters += UpdateActivation;
		}

		void OnDestroy()
		{
			if(Application.isPlaying == false)
				return;

			activator_master.onUpdateActivation_withParameters -= UpdateActivation;
		}

#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			LazyGetActivator();

			if(activator_slave == null)
				return;

			if(activator_master == null)
				return;

			UpdateSlave();
		}
#endif

		void LazyGetActivator()
		{
			if(activator_slave == null)
				activator_slave = GetComponent<Activator>();
		}

		void UpdateSlave()
		{
			activator_slave.SelectedIndex = activator_master.SelectedIndex;
		}

		void UpdateActivation(int selectedIndex, bool setFirstState = false)
		{
			activator_slave.ForceUpdateActivation(selectedIndex, setFirstState);
		}
	}
}