using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.iOS.Xcode;
using UnityEditor;
using System.IO;

public class PlistUtility
{
    public static bool TryGetPlist(string plistPath, out PlistDocument plistDoc)
    {
        plistDoc = new PlistDocument();

        try
        {
            plistDoc.ReadFromFile(plistPath);
        }

        catch
        {
            plistDoc = null;
            return false;
        }

        return true;
    }

    public static void SetKey(PlistElementDict dict, string key, bool val)
    {
        if (!dict.values.ContainsKey(key))
            dict.CreateArray(key);
        dict.SetBoolean(key, val);
    }

    public static void SetKey(PlistElementDict dict, string key, string val)
    {
        if (!dict.values.ContainsKey(key))
            dict.CreateArray(key);
        dict.SetString(key, val);
    }

    public static void DeleteKey(PlistElementDict dict, string key)
    {
        if (!dict.values.ContainsKey(key))
            return;
        dict.values.Remove(key);
    }

    public static PlistElementDict GetDict(PlistElementDict sourceDict, string key)
    {
        PlistElement element;

        if (!sourceDict.values.TryGetValue(key, out element))
            return sourceDict.CreateDict(key);
        else
            return element.AsDict();
    }
}
