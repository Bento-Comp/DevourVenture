using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class IdleNumber : System.IComparable<IdleNumber>
{
    public double m_value;
    public int m_exp;


    public IdleNumber(double value, int exp)
    {
        m_value = value;
        m_exp = exp;
    }

    public IdleNumber(IdleNumber idleNumber)
    {
        m_value = idleNumber.m_value;
        m_exp = idleNumber.m_exp;
    }


    public static IdleNumber operator +(IdleNumber a, IdleNumber b)
    {
        IdleNumber smaller = FindSmaller(a, b);
        IdleNumber greater = smaller == a ? b : a;

        IdleNumber convertedSmaller = IdleNumberConvert(smaller, greater.m_exp);


        return FormatIdleNumber(new IdleNumber(greater.m_value + convertedSmaller.m_value, greater.m_exp));
    }

    public static IdleNumber operator -(IdleNumber a, IdleNumber b)
    {
        IdleNumber smaller = FindSmaller(a, b);
        IdleNumber greater = smaller == a ? b : a;

        IdleNumber convertedSmaller = IdleNumberConvert(smaller, greater.m_exp);

        if (greater == a)
            return FormatIdleNumber(new IdleNumber(greater.m_value - convertedSmaller.m_value, greater.m_exp));
        else
            return FormatIdleNumber(new IdleNumber(convertedSmaller.m_value - greater.m_value, greater.m_exp));
    }

    public static IdleNumber operator *(IdleNumber a, float b)
    {
        return FormatIdleNumber(new IdleNumber(a.m_value * b, a.m_exp));
    }

    public static IdleNumber operator /(IdleNumber a, float b)
    {
        if (b == 0)
        {
            Debug.LogError("Cannot divide by 0");
            return a;
        }

        if (a.m_value == 1 && b == 0 && a.m_value < b)
            return a;


        return FormatIdleNumber(new IdleNumber(a.m_value / b, a.m_exp));
    }



    /// <summary>
    /// Convert an idle number to a new idle number with the target exp value
    /// Example : IdleNumber(50, 1) with a terget of 2
    /// Result will be IdleNumber(5, 2)
    /// </summary>
    /// <param name="a"></param>
    /// <param name="targetExp"></param>
    /// <returns></returns>
    public static IdleNumber IdleNumberConvert(IdleNumber a, int targetExp)
    {
        if (a.m_exp == targetExp)
            return a;

        int expDiff = targetExp - a.m_exp;

        double value = a.m_value / Mathf.Pow(10, expDiff);

        return new IdleNumber(value, targetExp);
    }


    public static IdleNumber FindSmaller(IdleNumber a, IdleNumber b, bool canReturnNull = false)
    {
        if (a.m_exp > b.m_exp)
            return b;
        else if (a.m_exp < b.m_exp)
            return a;
        else
        {
            if (a.m_value > b.m_value)
                return b;
            else if (a.m_value < b.m_value)
                return a;
        }

        if (canReturnNull)
            return null;

        return a;
    }

    public static IdleNumber FindGreater(IdleNumber a, IdleNumber b)
    {
        IdleNumber tmp = FindSmaller(a, b);

        if (tmp == a)
            return b;
        else
            return a;
    }

    public int CompareTo(IdleNumber idleNumberToCompare)
    {
        // A null value means that this object is greater.
        if (idleNumberToCompare == null)
            return 1;
        else
            return CompareIdleNumbers(this, idleNumberToCompare);
    }

    public static int CompareIdleNumbers(IdleNumber a, IdleNumber b)
    {
        IdleNumber tmp = FindGreater(a, b);

        if (tmp == a)
            return 1;
        else if (tmp == b)
            return -1;
        else
            return 0;
    }


    public static IdleNumber FormatIdleNumber(IdleNumber a)
    {
        if (a.m_value == 0 && a.m_exp == 0)
            return a;

        while (a.m_exp % 3 != 0)
        {
            a.m_exp++;
            a.m_value /= 10;
        }

        while (DoubleUtils.AbsoluteValueOf(a.m_value) < 1 && a.m_exp >= 3)
        {
            a.m_value *= 1000;
            a.m_exp -= 3;
        }

        while (DoubleUtils.AbsoluteValueOf(a.m_value) > 1000)
        {
            a.m_value /= 1000;
            a.m_exp += 3;
        }

        return a;
    }

    public static void PrintIdleNumber(IdleNumber a)
    {
        Debug.Log("(" + a.m_value + ", " + a.m_exp + ")");
    }

    public static string FormatIdleNumberText(IdleNumber a)
    {
        a = FormatIdleNumber(a);

        int unitRange = a.m_exp / 3;

        double value;
        string valueText = "";

        if (a.m_value < 1f && a.m_exp <= 0)
            return a.m_value.ToString("F2");

        if (a.m_exp % 3 != 0)
            value = a.m_value / Mathf.Pow(10, 3 - a.m_exp % 3);
        else
            value = a.m_value;

        if (value / 10f < 1)
            valueText = value.ToString("F2");
        else if (value / 100f < 1)
            valueText = value.ToString("F1");
        else
            valueText = value.ToString("F0");

        string suffixe = "";

        if (unitRange <= 0)
            suffixe = "";
        else if (unitRange == 1)
            suffixe = "K";
        else if (unitRange == 2)
            suffixe = "M";
        else if (unitRange == 3)
            suffixe = "B";
        else if (unitRange == 4)
            suffixe = "T";
        else
            suffixe = DetermineUnitScale(unitRange - 5);

        return valueText + suffixe;
    }


    public static string DetermineUnitScale(int index)
    {
        if (index < 0)
        {
            Debug.LogError("Cannot determine unit scale because index is negative : " + (index - 5).ToString());
            return "";
        }

        if (index == 0)
            return "aa";

        int prefixIndex = index / 26;
        int suffixIndex = index % 26;

        char prefix = (char)(((int)'a') + prefixIndex);
        char suffix = (char)(((int)'a') + suffixIndex);

        return prefix.ToString() + suffix.ToString();
    }


}
