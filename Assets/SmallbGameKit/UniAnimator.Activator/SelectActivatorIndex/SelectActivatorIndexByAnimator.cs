using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniActivation;

namespace UniAnimator
{
	[AddComponentMenu("UniAnimator/SelectActivatorIndexByAnimator")]
	public class SelectActivatorIndexByAnimator : MonoBehaviour
	{
		public Activator activator;

		public void SelectActivatorIndex(int index)
		{
			activator.SelectedIndex = index;
		}
	}
}
