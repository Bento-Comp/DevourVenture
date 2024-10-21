using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Juicy;

namespace JuicyInternal
{
    public enum JuicyABTestType
    {
        Mediation,
        Custom
    }

    [DefaultExecutionOrder(-32000)]
    [AddComponentMenu("JuicySDKInternal/JuicyABTestManager")]
    public class JuicyABTestManager : MonoBehaviour
    {
        static JuicyABTestManager instance;
        public static JuicyABTestManager Instance
        {
            get
            {
                return instance;
            }
        }

        int cohortIndexSave { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.COHORT_INDEX, -1); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.COHORT_INDEX, value); } }
        int currentCohortIndex = -1;
        public int CohortIndex
        {
            get
            {
                #if UNITY_EDITOR
                if (JuicySDKSettings.Instance.ForceABTestVariantInEditor)
                    return JuicySDKSettings.Instance.AbTestVariantToForce;
                #elif debugJuicySDK
                if(JuicySDKSettings.Instance.ForceAbTestVariantInBuild)
                    return JuicySDKSettings.Instance.AbTestVariantToForce;
                #endif
                return currentCohortIndex;

            }
        }

        void Awake()
        {
            if (instance != null)
            {
                if (instance != this)
                    Destroy(gameObject);

                return;
            }

            instance = this;

            if (JuicySnapshot.FirstInstallAppVersion == "undefined")
                OnFirstInstall();
            else if (JuicySnapshot.FirstInstallAppVersion != Application.version)
                OnVersionUpdate();

            currentCohortIndex = cohortIndexSave;
            JuicySDKLog.Log("JuicyAbTestManager : Awake : CohortIndex : " + currentCohortIndex);
        }

        void OnFirstInstall()
        {
            cohortIndexSave = GetCohortIndex();
        }

        void OnVersionUpdate()
        {
            if (JuicySDKSettings.Instance.EnableAbTest && JuicySDKSettings.Instance.AbTestKeepVariantIndex)
                return;

            cohortIndexSave = -1;
        }

        int GetCohortIndex()
        {
            if (!JuicySDKSettings.Instance.EnableAbTest)
                return -1;

            Random.State oldState = Random.state;
            Random.InitState((int)System.DateTime.Now.Ticks);
            float randomValue = Random.value;
            Random.state = oldState;

            int variantAmount = JuicySDKSettings.Instance.AbTestVariantAmount;
            float variantPopulation = (float)JuicySDKSettings.Instance.AbTestVariantPopulation / 100;

            for (int i = 0; i < variantAmount + 1; i++)
            {
                if (randomValue < ((i + 1) * variantPopulation))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}