using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{
	public interface IActivatorList
	{
		int ActivatorCount {get;}
		List<Activator> Activators {get;}

		void AddListChangeListener(System.Action onListChange);
		void RemoveListChangeListener(System.Action onListChange);
	}
}
