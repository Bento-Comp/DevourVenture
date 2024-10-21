using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalUpgradeSlot : MonoBehaviour
{
    public static System.Action<GameObject, GlobalUpgrade> OnButtonPress_TryToMakeUpgrade;


    [SerializeField]
    private GameObject m_rootObject = null;

    [SerializeField]
    private UI_Generic_Animator_Controller m_upgradeSlotUI_AnimatorController = null;

    [SerializeField]
    private Button m_upgradeButton = null;

    [SerializeField]
    private Image m_upgradeImage = null;

    [SerializeField]
    private TMP_Text m_upgradeTitle = null;

    [SerializeField]
    private TMP_Text m_upgradeDescription = null;

    [SerializeField]
    private TMP_Text m_upgradeCost = null;


    [SerializeField]
    private Color m_purchasableColor = Color.white;

    [SerializeField]
    private Color m_notPurchasableColor = Color.red;

    [SerializeField]
    private Image m_buttonImage = null;

    [SerializeField]
    private Image m_buttonBackgroundImage = null;

    [SerializeField]
    private Color m_buttonDisabledColor = Color.white;

    [SerializeField]
    private Color m_buttonDisabledBackgroundColor = Color.grey;

    [SerializeField]
    private Color m_buttonEnabledColor = Color.blue;

    [SerializeField]
    private Color m_buttonEnabledBackgroundColor = Color.cyan;


    private GlobalUpgrade m_buttonGlobalUpgrade;



    private void OnEnable()
    {
        Manager_GlobalUpgradesUI.OnButtonActivated += OnButtonActivated;
        m_upgradeButton.onClick.AddListener(ButtonPress_TryToUnlockGLobalUpgrade);
        Manager_Money.OnUpdateMoney += UpdateMoney;
        Manager_GlobalUpgrades.OnGlobalUpgradePurchased += OnGlobalUpgradePurchased;

        m_upgradeSlotUI_AnimatorController.OnAnimationEnd_Disapear += DisableUI;
    }

    private void OnDisable()
    {
        Manager_GlobalUpgradesUI.OnButtonActivated -= OnButtonActivated;
        m_upgradeButton.onClick.RemoveListener(ButtonPress_TryToUnlockGLobalUpgrade);
        Manager_Money.OnUpdateMoney -= UpdateMoney;
        Manager_GlobalUpgrades.OnGlobalUpgradePurchased -= OnGlobalUpgradePurchased;

        m_upgradeSlotUI_AnimatorController.OnAnimationEnd_Disapear -= DisableUI;
    }

    private void OnGlobalUpgradePurchased(GameObject buttonReference)
    {
        if (m_rootObject == buttonReference)
        {
            m_upgradeSlotUI_AnimatorController.Play_Disapear();
        }
    }

    private void DisableUI()
    {
        m_rootObject.SetActive(false);
    }

    private void OnButtonActivated(GameObject buttonReference, GlobalUpgrade globalUpgrade)
    {
        if (buttonReference == m_rootObject)
        {
            if (Manager_GlobalUpgrades.Instance.GetPurchasableGlobalUpgrade(globalUpgrade).IsPurchased == true)
            {
                DisableUI();
                return;
            }

            m_upgradeSlotUI_AnimatorController.Play_Reset();

            m_buttonGlobalUpgrade = globalUpgrade;

            m_upgradeImage.sprite = globalUpgrade.m_sprite;
            m_upgradeTitle.text = globalUpgrade.m_tittledescription;
            m_upgradeDescription.text = globalUpgrade.m_effectDescription;
            m_upgradeCost.text = IdleNumber.FormatIdleNumberText(globalUpgrade.m_cost_IdleNumber);

            UpdateMoney();
        }
    }

    private void UpdateMoney()
    {
        if (m_buttonGlobalUpgrade != null && Manager_Money.Instance.HasEnoughMoney(m_buttonGlobalUpgrade.m_cost_IdleNumber))
        {
            m_upgradeCost.color = m_purchasableColor;
            m_buttonImage.color = m_buttonEnabledColor;
            m_buttonBackgroundImage.color = m_buttonEnabledBackgroundColor;
            m_upgradeButton.interactable = true;
        }
        else
        {
            m_upgradeCost.color = m_notPurchasableColor;
            m_buttonImage.color = m_buttonDisabledColor;
            m_buttonBackgroundImage.color = m_buttonDisabledBackgroundColor;
            m_upgradeButton.interactable = false;
        }
    }

    public void ButtonPress_TryToUnlockGLobalUpgrade()
    {
        OnButtonPress_TryToMakeUpgrade?.Invoke(m_rootObject, m_buttonGlobalUpgrade);
    }
}
