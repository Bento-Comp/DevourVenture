using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Juicy;

namespace JuicyInternal { 
public class WelcomePrivacySettingsPopUpV1 : PrivacySettingsPopUp
{
    #pragma warning disable CS0649
    [SerializeField] Text Label;
    [SerializeField] Toggle AdsToggle;
    [SerializeField] Toggle AnalyticsToggle;
    [SerializeField] Toggle AgeToggle;
    [SerializeField] Transform linkHolder;
    [SerializeField] Button_PartnerPrivacyPolicyLink partnerLinkPrefab;
    #pragma warning restore CS0649

    public override void Awake()
    {
        base.Awake();
        SetUpUI();
        SetUpPartnerPage();
        Label.text = Application.productName + " GDPR";
    }

    public override void Close()
    {
        base.Close();
        Destroy(gameObject);
    }

    void SetUpUI()
    {
        AdsToggle.isOn = false;
        AnalyticsToggle.isOn = false;
        AgeToggle.isOn = false;
    }

    void SetUpPartnerPage()
    {
        foreach (string link in JuicyBasePrivacyDatas.PrivacyPolicyLinks)
        {
            Button_PartnerPrivacyPolicyLink bLink = Instantiate(partnerLinkPrefab);
            bLink.transform.SetParent(linkHolder);
            bLink.url = link;
        }

#if !noJuicyCompilation
            foreach (string link in JuicyMediationPrivacyDatas.PrivacyPolicyLinks)
            {
                Button_PartnerPrivacyPolicyLink bLink = Instantiate(partnerLinkPrefab);
                bLink.transform.SetParent(linkHolder);
                bLink.url = link;
            }
#endif
    }

    public void OnAcceptButtonClick()
    {
        JuicyPrivacyManager.Instance.OnWelcomePopUpCompleted(true, true, true);
        Close();
    }

    public void OnAcceptSettingsButtonClick()
    {
            
        JuicyPrivacyManager.Instance.OnWelcomePopUpCompleted(AdsToggle.isOn, AnalyticsToggle.isOn, AgeToggle.isOn);
        Close();
    }

    public void OnSettingsButtonClick()
    {
        OpenPage(1);
    }

    public void OnBackButtonClick()
    {
        OpenPage(0);
    }
}
}

