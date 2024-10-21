using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JuicyInternal
{
    public class UpdatePrivacySettingsPopUpV2 : PrivacySettingsPopUp
    {
#pragma warning disable CS0649
        [SerializeField] Text subTitle;
        [SerializeField] Toggle AdsToggle;
        [SerializeField] Toggle AnalyticsToggle;
        [SerializeField] Transform adsLinkHolder;
        [SerializeField] Transform analyticsLinkHolder;
        [SerializeField] Button_PartnerPrivacyPolicyLink partnerLinkPrefab;

        //Manage datas pages
        [SerializeField] Button manageDatasButton;
        [SerializeField] Text idfaGaidText;
        #pragma warning restore CS0649

        public override void Awake()
        {
            base.Awake();
            SetUpPartnerPage();
        }

        public override void Open()
        {
            base.Open();
            SetUpUI();
        }

        private void OnEnable()
        {
            AdsToggle.isOn = JuicyPrivacyManager.AdsEnabled;
            AnalyticsToggle.isOn = JuicyPrivacyManager.AnalyticsEnabled;
        }

        public void CloseAndSavePreferences()
        {
            JuicyPrivacyManager.Instance.UpdatePrivacySettings(AdsToggle.isOn, AnalyticsToggle.isOn, true);
            Close();
        }

        void SetUpUI()
        {
            bool lat = JuicyPrivacyManager.GetLATStatus();

            AdsToggle.isOn = JuicyPrivacyManager.AdsEnabled;
            AnalyticsToggle.isOn = JuicyPrivacyManager.AnalyticsEnabled;

#if UNITY_IOS
            AdsToggle.interactable = !lat;
            AnalyticsToggle.interactable = !lat;


            if (lat)
            {
                subTitle.text = "You have disabled ad tracking on your device.";
                subTitle.color = Color.red;
            }
#endif

            manageDatasButton.gameObject.SetActive(!lat);
            idfaGaidText.text = JuicyPrivacyManager.GetAdIdentifiant();
        }

        void SetUpPartnerPage()
        {
            foreach (string link in JuicyBasePrivacyDatas.PrivacyPolicyLinks)
            {
                Button_PartnerPrivacyPolicyLink bLink = Instantiate(partnerLinkPrefab);
                bLink.transform.SetParent(analyticsLinkHolder);
                bLink.url = link;
            }

            #if !noJuicyCompilation
            foreach (string link in JuicyMediationPrivacyDatas.PrivacyPolicyLinks)
            {
                Button_PartnerPrivacyPolicyLink bLink = Instantiate(partnerLinkPrefab);
                bLink.transform.SetParent(adsLinkHolder);
                bLink.url = link;
            }
            #endif
        }

        public void OnManageDatasButtonClick()
        {
            screenController.SelectScreen(1);
        }

        public void OnManageDatasBackButtonClick()
        {
            screenController.SelectScreen(0);
        }

        public void OnAccessDatasButtonClick()
        {
            JuicyPrivacyManager.Instance.SendAccessDatasMail();
        }

        public void OnDeleteDatasButtonClick()
        {
            JuicyPrivacyManager.Instance.SendForgetMeMail();
        }
    }
}
