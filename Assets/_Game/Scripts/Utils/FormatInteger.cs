using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormatInteger
{
    public static string FormatInt(long number)
    {
        float tmp = 0;
        string suffixe = "";

        if (number / 1000000000f >= 1)
        {
            tmp = number / 1000000000f;
            suffixe = "B";
        }
        else if (number / 1000000f >= 1)
        {
            tmp = number / 1000000f;
            suffixe = "M";
        }
        else if (number / 1000f >= 1)
        {
            tmp = number / 1000f;
            suffixe = "K";
        }
        else
            return number.ToString();


        if (tmp / 10f < 1)
            return tmp.ToString("F2") + suffixe;
        else if (tmp / 100f < 1)
            return tmp.ToString("F1") + suffixe;
        else
            return tmp.ToString("F0") + suffixe;

    }
}
