using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[ExecuteAlways]
	[AddComponentMenu("UniActivation/Activator_Master")]
	public class Activator_Master : MonoBehaviour
	{
		public Activator activator_slave;

		Activator activator_master;

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
			if(activator_master == null)
				activator_master = GetComponent<Activator>();
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