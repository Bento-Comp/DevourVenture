using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalUpgrade 
{
    public Bonus m_bonus;
    public FoodType m_foodType;
    public Sprite m_sprite;
    public IdleNumber m_cost_IdleNumber;

    [Tooltip("Value used to know The profit multiplier of the food")]
    public int m_profitMultiplier = 1;

    [Tooltip("Value used to know how many people to spawn (worker, customer, cashier and chefs)")]
    public int m_count = 1;
    public string m_tittledescription;
    public string m_effectDescription;


    public int CompareTo(GlobalUpgrade upgradeToCompare)
    {
        // A null value means that this object is greater.
        if (upgradeToCompare == null)
            return 1;

        else
            return this.m_cost_IdleNumber.CompareTo(upgradeToCompare.m_cost_IdleNumber);
    }
}
