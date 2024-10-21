using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Juicy;

namespace JuicyInternal
{
    public class WelcomePrivacySettingsPopUpV2 : PrivacySettingsPopUp
    {
#pragma warning disable CS0649
        [Header("Reference")]
        [SerializeField] Text Label;
        [SerializeField] Text ExplanationText;
        [SerializeField] Toggle AdsToggle;
        [SerializeField] Toggle AnalyticsToggle;
        [SerializeField] Toggle AgeToggle;
        [SerializeField] Button AcceptButton;
        [SerializeField] Transform linkHolder;
        [Header("Prefabs")]
        [SerializeField] Button_PartnerPrivacyPolicyLink partnerLinkPrefab;
        [Header("LAT")]
        [SerializeField] List<GameObject> noLATList;
        [SerializeField] List<GameObject> LATList;
#pragma warning restore CS0649

        public override void Awake()
        {
            base.Awake();
            SetUpUI();
            SetUpPartnerPage();
            Label.text = "Thanks for downloading" + System.Environment.NewLine + Application.productName;
        }

        public override void Close()
        {
            base.Close();
            Destroy(gameObject);
        }

        void SetUpUI()
        {
            AdsToggle.isOn= false;
            AnalyticsToggle.isOn = false;
            AgeToggle.isOn = false;
            AcceptButton.interactable = false;

#if UNITY_IOS
            bool isLat = JuicyPrivacyManager.GetLATStatus();
            foreach (GameObject obj in noLATList)
                obj.SetActive(!isLat);
            foreach (GameObject obj in LATList)
                obj.SetActive(isLat);
#endif

            AgeToggle.onValueChanged.AddListener((bool b) => CheckForAcceptButtonInteractable());
            AdsToggle.onValueChanged.AddListener((bool b) => CheckForAcceptButtonInteractable());
            AnalyticsToggle.onValueChanged.AddListener((bool b) => CheckForAcceptButtonInteractable());

            AcceptButton.onClick.AddListener(OnAcceptButtonClick);
        }

        void SetUpPartnerPage()
        {
            foreach(string link in JuicyBasePrivacyDatas.PrivacyPolicyLinks)
            {
                Button_PartnerPrivacyPolicyLink bLink = Instantiate(partnerLinkPrefab);
                bLink.transform.SetParent(linkHolder);
                bLink.url = link;
            }

#if !noJuicyCompilation
            GameObject title = Instantiate(linkHolder.GetChild(0).gameObject);
            title.transform.SetParent(linkHolder);
            title.GetComponent<Text>().text = System.Environment.NewLine + System.Environment.NewLine + "Advertisement";

            foreach (string link in JuicyMediationPrivacyDatas.PrivacyPolicyLinks)
            {
                Button_PartnerPrivacyPolicyLink bLink = Instantiate(partnerLinkPrefab);
                bLink.transform.SetParent(linkHolder);
                bLink.url = link;
            }
#endif
        }

        void CheckForAcceptButtonInteractable()
        {
            AcceptButton.interactable = AgeToggle.isOn;
        }

        void OnAcceptButtonClick()
        {
            JuicyPrivacyManager.Instance.OnWelcomePopUpCompleted(AdsToggle.isOn, AnalyticsToggle.isOn, AgeToggle.isOn);
            Close();
        }

        public void OnPrivacyButtonClick()
        {
            Application.OpenURL(JuicySDK.Settings.BaseConfig.PrivacyPolicyLink);
        }

        public void OnPartnerButtonClick()
        {
            OpenPage(1);
        }

        public void OnBackButtonClick()
        {
            OpenPage(0);
        }
    }
}
