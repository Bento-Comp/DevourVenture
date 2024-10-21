using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniActivation
{
    [ExecuteInEditMode()]
    [DefaultExecutionOrder(-32000)]
    [AddComponentMenu("UniActivation/NamedActivationsRegister")]
    public class NamedActivationsRegister : MonoBehaviour
    {
        public System.Action onNamedActivationChanged;

		public NamedActivationsRegister parentRegister;

        Dictionary<string, HashSet<NamedActivation>> activationsByNames = new Dictionary<string, HashSet<NamedActivation>>();

        public bool IsActive(string activationName)
        {
            if(activationName == "")
                return false;

            bool isActive = activationsByNames.ContainsKey(activationName);

			if(isActive == false && parentRegister != null)
				isActive = parentRegister.IsActive(activationName);

            return isActive;
        }

        public void Register(NamedActivation namedActivation, string activationName)
        {
			HashSet<NamedActivation> namedActivations;
            if(activationsByNames.TryGetValue(activationName, out namedActivations) == false)
            {
				namedActivations = new HashSet<NamedActivation>();
				activationsByNames.Add(activationName, namedActivations);
            }

            namedActivations.Add(namedActivation);

            OnNamedActivationChanged();
        }

        public void Unregister(NamedActivation namedActivation, string activationName)
        {
            HashSet<NamedActivation> namedActivations;
            if(activationsByNames.TryGetValue(activationName, out namedActivations))
            {
                namedActivations.Remove(namedActivation);

				if(namedActivations.Count <= 0)
                {
                    activationsByNames.Remove(activationName);
                }
            }

            OnNamedActivationChanged();
        }

        void OnNamedActivationChanged()
        {
            onNamedActivationChanged?.Invoke();
        }
    }
}