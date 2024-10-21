#if !noJuicyCompilation
using UnityEngine;
using System.Xml;
using Juicy;
using JuicyInternal;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Android;
using System.IO;
using System.Collections.Generic;
using Facebook.Unity.Editor;
using Facebook.Unity.Settings;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class JuicyMediationProcessBuildManager : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
#if UNITY_ANDROID
    private string androidNamespace = "http://schemas.android.com/apk/res/android";
    private string adMobManifestPath { get { return Application.dataPath + "/Plugins/Android/MaxMediationGoogle.androidlib/AndroidManifest.xml"; } }
#endif

    //Make sure plist or manifest edition is done after everything else
    public int callbackOrder { get { return 32000; } }

    public void OnPreprocessBuild(BuildReport report)
    {
#if UNITY_ANDROID
        AddAdMobIDToManifest();
#endif
    }

    public void OnPostprocessBuild(BuildReport report)
    {
#if UNITY_IOS
        EditAppPlist(report.summary.outputPath + "/Info.plist");
#endif
    }

#if UNITY_ANDROID
    void AddAdMobIDToManifest()
    {
        XmlDocument manifest = new XmlDocument();
        if (!XmlUtility.TryGetXmlDocument(adMobManifestPath, out manifest))
        {
            Debug.LogError("JuicyMediationProcessBuildManager : AddAdMobIDToManifest : Can't find manifest at : " + adMobManifestPath);
            return;
        }

        XmlNode appNode = XmlUtility.GetNode(manifest, "application");

        //Find the node with right name and name attribute or add it if it doesn't exist
        XmlNode adMobNode = XmlUtility.GetNode(appNode, "meta-data", "android:name", "com.google.android.gms.ads.APPLICATION_ID");
        if (adMobNode == null)
        {
            adMobNode = XmlUtility.AddNode(manifest, appNode, "meta-data");
            XmlUtility.SetNodeAttribute(manifest, adMobNode, "android", "name", androidNamespace, "com.google.android.gms.ads.APPLICATION_ID");
        }
        //Set the value attribute
        XmlUtility.SetNodeAttribute(manifest, adMobNode, "android", "value", androidNamespace, JuicySDKSettings.Instance.MediationConfig.AdmobAppID);

        manifest.Save(adMobManifestPath);
    }
#endif

#if UNITY_IOS
    void EditAppPlist(string plistPath)
    {
        PlistDocument plist = new PlistDocument();
        if (!PlistUtility.TryGetPlist(plistPath, out plist))
        {
            Debug.LogError("JuicyMediationProcessBuildManager : EditAppPlist : Can't find plist at : " + plistPath);
            return;
        }

        PlistElementDict rootDict = plist.root;

        /*--- Ad Mob ID ---*/
        PlistUtility.SetKey(rootDict, "GADApplicationIdentifier", JuicySDKSettings.Instance.MediationConfig.AdmobAppID);
        plist.WriteToFile(plistPath);
    }
#endif
}
#endif
