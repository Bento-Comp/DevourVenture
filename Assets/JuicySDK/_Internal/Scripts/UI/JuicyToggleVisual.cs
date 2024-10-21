using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JuicyInternal
{
    public class JuicyToggleVisual : MonoBehaviour
    {
        [SerializeField] GameObject offHandle;
        [SerializeField] GameObject onHandle;
        [SerializeField] Image background;
        [SerializeField] Color offColor;
        [SerializeField] Color onColor;
        Toggle toggle;

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleStateChange);
        }

        private void OnEnable()
        {
            OnToggleStateChange(toggle.isOn);
        }

        private void OnDestroy()
        {
            toggle.onValueChanged.RemoveListener(OnToggleStateChange);
        }

        void OnToggleStateChange(bool b)
        {
            offHandle.SetActive(!b);
            onHandle.SetActive(b);
            background.color = b ? onColor : offColor;
        }
    }
}
