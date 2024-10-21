using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniPool
{
	[AddComponentMenu("UniPool/PoolBase")]
	public abstract class PoolBase : MonoBehaviour
	{
		public virtual void DestroyPoolInstance(PoolInstance instance)
		{
			if(instance.CanDestroy == false)
				return;
			
			instance.transform.SetParent(transform, false);
			instance.transform.localPosition = Vector3.zero;
			instance.PoolInstanceIsDestroyed = true;
			instance.gameObject.SetActive(false);
		}
	}
}