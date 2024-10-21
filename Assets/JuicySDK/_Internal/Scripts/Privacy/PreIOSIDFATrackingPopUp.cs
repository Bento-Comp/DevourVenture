using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JuicyInternal
{
    public class PreIOSIDFATrackingPopUp : MonoBehaviour
    {
        #pragma warning disable CS0649
        [SerializeField] Button okButton;
        #pragma warning restore CS0649

        private void Awake()
        {
            okButton.onClick.AddListener(OnOkButtonClick);
        }

        void OnOkButtonClick()
        {
#if UNITY_IOS
            JuicyPrivacyManager.Instance.CallATTPopUp();
#endif
            Destroy(gameObject);
        }
    }
}
