using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Juicy;

namespace JuicyInternal
{
    [AddComponentMenu("JuicySDKInternal/Button_PrivacyPolicyLink")]
    [ExecuteInEditMode()]
    public class Button_PrivacyPolicyLink : PrivacyButtonBase
    {
        public string url
        {
            get
            {
                return JuicySDK.Settings.BaseConfig.PrivacyPolicyLink;
            }
        }

        public Text textComponent;

        protected override void OnClick()
        {
            Application.OpenURL(url);
        }

        private void Start()
        {
            UpdateText();
        }

#if UNITY_EDITOR
        void LateUpdate()
        {
            if (Application.isPlaying)
                return;

            UpdateText();
        }
#endif

        void UpdateText()
        {
            if (textComponent == null)
                return;

            textComponent.text = url;
        }
    }
}