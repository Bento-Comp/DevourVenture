using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleUtils : MonoBehaviour
{
    public static double AbsoluteValueOf(double number)
    {
        double num = number;
        if (number < 0)
            num = -1 * number;
        return num;
    }
}
