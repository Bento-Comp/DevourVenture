using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Food Visual Assets", order = 1)]
public class FoodVisualAssets_ScriptableObject : ScriptableObject
{
    public List<FoodVisualAssets> m_foodVisualAssetsList = null;
}
