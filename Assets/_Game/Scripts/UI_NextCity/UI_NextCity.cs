using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class CityInfo
{
    public Sprite m_cityImage;
    public string m_name;
    public int index;
}


public class UI_NextCity : Game_UI
{
    [SerializeField]
    private Animator m_uiPlaneAnimator = null;

    [SerializeField]
    private List<CityInfo> m_cityInfoList = null;

    [SerializeField]
    private Image m_sourceLocationImage = null;

    [SerializeField]
    private TMP_Text m_sourceLocationText = null;

    [SerializeField]
    private Image m_destinationLocationImage = null;

    [SerializeField]
    private TMP_Text m_destinationLocationText = null;


    protected override void OnEnable()
    {
        base.OnEnable();
        LevelVisual.OnSendLevelVisualIndex += OnChangingCity;
    }


    protected override void OnDisable()
    {
        base.OnDisable();
        LevelVisual.OnSendLevelVisualIndex -= OnChangingCity;
    }

    private void Start()
    {
        ToggleUI(false);
    }

    private void OnChangingCity(int levelVisualIndex)
    {
        OpenUI();

        m_destinationLocationImage.sprite = m_cityInfoList[levelVisualIndex].m_cityImage;
        m_destinationLocationText.text = m_cityInfoList[levelVisualIndex].m_name;

        int previousIndex = levelVisualIndex-1;
        if (previousIndex < 0)
            previousIndex = m_cityInfoList.Count - 1;

        m_sourceLocationImage.sprite = m_cityInfoList[previousIndex].m_cityImage;
        m_sourceLocationText.text = m_cityInfoList[previousIndex].m_name;
    }

}
