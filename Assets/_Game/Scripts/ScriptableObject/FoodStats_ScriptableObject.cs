using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Food Stats", order = 1)]
public class FoodStats_ScriptableObject : ScriptableObject
{
    public List<FoodStats> m_foodStatList = null;
}
