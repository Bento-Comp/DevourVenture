using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customer_UI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_customerUIObject = null;

    [SerializeField]
    private Customer m_customer = null;

    [SerializeField]
    private Image m_foodSprite = null;

    [SerializeField]
    private TMP_Text m_quantityText = null;

    private FoodVisualAssets m_currentFoosdVisualAssets;
    private FoodVisualAssets m_foodVisualAssetsBuffer;


    private void OnEnable()
    {
        m_customer.OnUpdateOrderUI += OnUpdateOrderUI;
    }

    private void OnDisable()
    {
        m_customer.OnUpdateOrderUI -= OnUpdateOrderUI;
    }


    private void OnUpdateOrderUI(Order order)
    {
        if (order.m_remainingQuantityToServe == 0)
        {
            m_customerUIObject.SetActive(false);
            return;
        }


        if (!m_customerUIObject.activeInHierarchy)
            m_customerUIObject.SetActive(true);


        if (m_currentFoosdVisualAssets == null)
            m_foodVisualAssetsBuffer = Manager_FoodVisualAssets.Instance.GetFoodVisualAsset(order.m_foodType);


        m_quantityText.text = order.m_remainingQuantityToServe.ToString();


        if (m_currentFoosdVisualAssets != m_foodVisualAssetsBuffer)
        {
            m_currentFoosdVisualAssets = m_foodVisualAssetsBuffer;

            m_foodSprite.sprite = m_currentFoosdVisualAssets.spriteImage;
        }
    }
}
