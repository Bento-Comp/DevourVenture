using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JuicySDKSample;

//WARNING
//Do not use this script or the JuicyManager > SkipInterstitialDelay function unless you're a member of the Juicy SDK development team
//WARNING

namespace JuicyInternal
{
    public class JuicySDKInternalTest : MonoBehaviour
    {
#if !debugJuicySDKInternal
        void Awake()
        {
            Destroy(this);
        }
#else
        Button button;
        JuicyAdsManager adsManager;

        void Awake()
        {
            adsManager = FindObjectOfType<JuicyAdsManager>();
            GameObject notifyButton = FindObjectOfType<Button_NotifyInterstitialOpportunity>().gameObject;

            if (notifyButton == null || adsManager == null)
            {
                Destroy(this);
                return;
            }

            GameObject skipButton = Instantiate(notifyButton, notifyButton.transform.parent);
            skipButton.transform.SetSiblingIndex(notifyButton.transform.GetSiblingIndex() + 1);
            Destroy(skipButton.GetComponent<Button_NotifyInterstitialOpportunity>());
            skipButton.GetComponentInChildren<Text>().text = "Skip Interstitial Delay";
            button = skipButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);

            skipButton.hideFlags = HideFlags.HideInHierarchy;
        }

        void OnDestroy()
        {
            if (button != null)
                button.onClick.RemoveListener(OnClick);
        }

        void OnClick()
        {
            if (adsManager == null)
                return;


#pragma warning disable CS0618 // Type or member is obsolete
            adsManager.SkipInterstitialDelay();
#pragma warning restore CS0618 // Type or member is obsolete
        }
#endif
    }
}
