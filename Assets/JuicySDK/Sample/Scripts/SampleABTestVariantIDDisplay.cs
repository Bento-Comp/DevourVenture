using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Juicy;

namespace JuicySDKSample
{
    public class SampleABTestVariantIDDisplay : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Text>().text = "AB Test Variant ID = " + JuicySDK.ABTestCohortVariantIndex;
        }
    }
}
