using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniPool
{
	[AddComponentMenu("UniPool/PoolInstance")]
	public class PoolInstance : MonoBehaviour
	{
		public System.Action onDestroyPoolInstance;

		bool canDestroy = true;

		bool destroyed;

		PoolBase pool;

		public bool IsPooled
		{
			get
			{
				return pool != null;
			}
		}

		public bool CanDestroy
		{
			get
			{
				return canDestroy && destroyed == false;
			}

			set
			{
				canDestroy = value;
			}
		}

		public bool PoolInstanceIsDestroyed
		{
			get
			{
				return destroyed;
			}

			set
			{
				destroyed = value;
			}
		}

		protected virtual void OnCreatePoolInstance()
		{
		}

		protected virtual void OnDestroyPoolInstance()
		{
		}

		public void CreatePoolInstance()
		{
			OnCreatePoolInstance();
		}

		public void DestroyPoolInstance()
		{
			if(canDestroy == false)
				return;
				
			if(pool != null)
			{
				pool.DestroyPoolInstance(this);
			}
			else
			{
				GameObject.Destroy(gameObject);
			}

			OnDestroyPoolInstance();

			if(onDestroyPoolInstance != null)
				onDestroyPoolInstance();
		}

		public virtual void Initialize(PoolBase pool)
		{
			this.pool = pool;
		}
	}
}