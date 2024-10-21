using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniActivation/Editor_Activator_Link")]
	public class Editor_Activator_Link : MonoBehaviour
	{
		public Activator activator_slave;

		public Activator activator_master;

		public bool reverse;

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

			int selectedIndex = activator_master.SelectedIndex;

			if(reverse)
			{
				selectedIndex = activator_master.IndexCount - selectedIndex - 1;
			}

			activator_slave.SelectedIndex = selectedIndex;
		}
#endif
	}
}