using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnum
{
    public static Enum GetRandomEnum<Enum>()
    {
        System.Array enumArray = System.Enum.GetValues(typeof(Enum));
        Enum randomEnum = (Enum)enumArray.GetValue(Random.Range(0, enumArray.Length));
        return randomEnum;
    }
}
