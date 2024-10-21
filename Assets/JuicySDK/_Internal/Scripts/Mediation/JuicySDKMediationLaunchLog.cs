using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuicySDKMediationLaunchLog
{
    public static string GetMediationLog()
    {
        string str = "";
        str += "MAX: " + MaxSdk.Version;
        return str;
    }
}
