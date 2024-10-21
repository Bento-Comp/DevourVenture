using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniActivation
{
	[AddComponentMenu("UniActivation/ActivationControllerBase")]
	public abstract class ActivationControllerBase : MonoBehaviour
	{
		public System.Action<ActivationControllerBase> onActiveChange;

		public System.Action<ActivationControllerBase> onDeactivationEnd;

		public System.Action<ActivationControllerBase> onActivationEnd;

		[SerializeField]
		bool active;

		public float activeDelay = 0.0f;

		public float deactiveDelay = 0.0f;

		bool activationInProgress;

		bool firstActive = true;

		ActivationControllerHub activationControllerHub;

		public ActivationControllerHub ActivationControllerHub
		{
			set
			{
				activationControllerHub = value;
			}
		}

		public bool ActivationInProgress
		{
			get
			{
				return activationInProgress;
			}
		}

		public bool FirstActive
		{
			get
			{
				return firstActive;
			}
		}

		public bool Active
		{
			set
			{ 
				#if UNITY_EDITOR
				if(Application.isPlaying == false)
				{
					active = value;
					OnEditorActiveChange();
				}
				else
				#endif
				{
					if(firstActive || active != value)
					{
						if(value)
						{
							RuntimeActive();
						}
						else
						{
							RuntimeDeactive();
						}
						firstActive = false;
					}
				}
			}

			get
			{
				return active;
			}
		}

		public void Activate(bool activate, bool setFirstState)
		{
			if(setFirstState)
			{
				SetFirstActiveState(activate);
			}
			else
			{
				Active = activate;
			}
		}

		protected abstract void OnSetFirstActiveState();

		protected abstract void OnActiveChange();

		#if UNITY_EDITOR
		protected abstract void OnEditorActiveChange();
		#endif

		protected void NotifyRuntimeDeactivationEnd()
		{
			if(activationControllerHub != null)
				activationControllerHub.NotifyRuntimeChildDeactivationEnd();
			
			OnDeactivationEnd();
			if(onDeactivationEnd != null)
			{
				onDeactivationEnd(this);
			}
		}

		protected void NotifyRuntimeActivationEnd()
		{
			activationInProgress = false;
			OnActivationEnd();
			if(onActivationEnd != null)
			{
				onActivationEnd(this);
			}
		}

		protected virtual void OnActivationEnd()
		{
		}

		protected virtual void OnDeactivationEnd()
		{
		}

		protected virtual void OnAwake()
		{
		}

		void Awake()
		{
			OnAwake();
			Active = active;
		}

		void OnDisable()
		{
			CancelInvoke();
		}

		void RuntimeActive()
		{
			CancelInvoke("DoRuntimeDeactive");
			CancelInvoke("DoRuntimeActive");
			if(activeDelay <= 0.0f)
			{
				DoRuntimeActive();
			}
			else
			{
				Invoke("DoRuntimeActive", activeDelay);
			}
		}

		void RuntimeDeactive()
		{
			CancelInvoke("DoRuntimeActive");
			CancelInvoke("DoRuntimeDeactive");
			if(deactiveDelay <= 0.0f)
			{
				DoRuntimeDeactive();
			}
			else
			{
				Invoke("DoRuntimeDeactive", deactiveDelay);
			}
		}

		void SetFirstActiveState(bool firstActiveState)
		{
			active = firstActiveState;
			OnSetFirstActiveState();
			if(onActiveChange != null)
			{
				onActiveChange(this);
			}
		}

		void DoRuntimeActive()
		{
			active = true;
			activationInProgress = true;
			OnActiveChange();
			if(onActiveChange != null)
			{
				onActiveChange(this);
			}
		}

		void DoRuntimeDeactive()
		{
			active = false;
			OnActiveChange();
			if(onActiveChange != null)
			{
				onActiveChange(this);
			}
		}
	}
}