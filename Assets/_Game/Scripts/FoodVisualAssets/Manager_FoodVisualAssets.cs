using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Manager_FoodVisualAssets : UniSingleton.Singleton<Manager_FoodVisualAssets>
{
    public FoodVisualAssets_ScriptableObject FoodVisualAssets_ScriptableObject = null;


    public FoodVisualAssets GetFoodVisualAsset(FoodType foodType)
    {
        for (int i = 0; i < FoodVisualAssets_ScriptableObject.m_foodVisualAssetsList.Count; i++)
        {
            if (FoodVisualAssets_ScriptableObject.m_foodVisualAssetsList[i].foodType == foodType)
            {
                return FoodVisualAssets_ScriptableObject.m_foodVisualAssetsList[i];
            }
        }

        return null;
    }
}
