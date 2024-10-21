using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JuicyInternal
{
    public class UpdatePrivacySettingsPopUpV1 : PrivacySettingsPopUp
    {
        #pragma warning disable CS0649
        [SerializeField] Toggle AdsToggle;
        [SerializeField] Toggle AnalyticsToggle;
        [SerializeField] Toggle AgeToggle;
        [SerializeField] Transform linkHolder;
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

        void SetUpUI()
        {
            AdsToggle.isOn = JuicyPrivacyManager.AdsEnabled;
            AnalyticsToggle.isOn = JuicyPrivacyManager.AnalyticsEnabled;
            AgeToggle.isOn = JuicyPrivacyManager.AgeEnabled;

            manageDatasButton.gameObject.SetActive(!JuicyPrivacyManager.GetLATStatus());
            idfaGaidText.text = JuicyPrivacyManager.GetAdIdentifiant();
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
            JuicyPrivacyManager.Instance.UpdatePrivacySettings(AdsToggle.isOn, AnalyticsToggle.isOn, AgeToggle.isOn);
            Close();
        }

        public void OnBackButtonClick()
        {
            Close();
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
