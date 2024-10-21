using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace JuicyInternal
{
    public class MediationDebuggerScene : MonoBehaviour
    {
        [SerializeField] Button reCallDebuggerButton;

        private void Awake()
        {
            Juicy.JuicySDK.HideBanner();
        }

        void Start()
        {
            reCallDebuggerButton.onClick.AddListener(ShowDebugger);
            ShowDebugger();
        }

        void ShowDebugger()
        {
            Debug.Log("Call");
            JuicyAdsManager.Instance.ShowMediationTestSuite();
        }
    }
}
