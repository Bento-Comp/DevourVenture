using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Juicy;
using System;
namespace JuicyInternal
{
    [System.Flags]
    public enum JuicySnapshotFlag
    {
        None = 0,

        Version = 1 << 0,
        Device = 1 << 1,
        Game = 1 << 2,
        Ads = 1 << 3,
        Time = 1 << 4,

        Complete = 0b_11111,
    }

    public class JuicySnapshot
    {
        /*-- Version --*/
        public static string FirstInstallAppVersion { get { return JuicyPlayerPrefs.GetString(JuicyPlayerPrefs.FIRST_INSTALL_APP_VERSION,"undefined"); } set { JuicyPlayerPrefs.SetString(JuicyPlayerPrefs.FIRST_INSTALL_APP_VERSION,value); } }
        public static string FirstInstallJuicyVersion { get { return JuicyPlayerPrefs.GetString(JuicyPlayerPrefs.FIRST_INSTALL_JUICY_VERSION, "undefined"); } set { JuicyPlayerPrefs.SetString(JuicyPlayerPrefs.FIRST_INSTALL_JUICY_VERSION, value); } }
        public static string CurrentInstallAppVersion { get { return Application.version; } }
        public static string CurrentInstallJuicyVersion { get { return JuicySDK.version; } }
        /*-- Analytics --*/
        //Game
        public static int LevelIndex { get { return JuicyAnalyticsManager.Instance.CurrentLevel; } }
        public static int GameCount { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.TOTAL_GAME_COUNT); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.TOTAL_GAME_COUNT,value); } }
        public static int SessionCount { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.TOTAL_SESSION_COUNT); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.TOTAL_SESSION_COUNT, value); } }
        //Ads
        public static int TotalBanner { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.TOTAL_BANNER_COUNT); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.TOTAL_BANNER_COUNT,value); } }
        public static int TotalInterstitial { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.TOTAL_INTERSTITIAL_COUNT); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.TOTAL_INTERSTITIAL_COUNT, value); } }
        public static int TotalRewarded { get { return JuicyPlayerPrefs.GetInt(JuicyPlayerPrefs.TOTAL_REWARDED_COUNT); } set { JuicyPlayerPrefs.SetInt(JuicyPlayerPrefs.TOTAL_REWARDED_COUNT, value); } }
        //Time
        public static float TotalGameTime { get { return JuicyPlayerPrefs.GetFloat(JuicyPlayerPrefs.TOTAL_GAME_TIME); } set { JuicyPlayerPrefs.SetFloat(JuicyPlayerPrefs.TOTAL_GAME_TIME, value); } }
        //Revenue
        public static float CurrentRevenue { get { return JuicyPlayerPrefs.GetFloat(JuicyPlayerPrefs.CURRENT_REVENUE); } set { JuicyPlayerPrefs.SetFloat(JuicyPlayerPrefs.CURRENT_REVENUE, value); } }

        /*-- Analytics --*/
        //Game
        int levelIndex;
        int gameCount;
        int sessionCount;
        //Ads
        int totalBanner;
        int totalInterstitial;
        int totalRewarded;
        //Time
        float totalGameTime;


        public JuicySnapshot(bool autoInit = true)
        {
            if (!autoInit)
                return;
            TakeSnaphshot();
        }

        public void TakeSnaphshot()
        {
            levelIndex = LevelIndex;
            gameCount = GameCount;
            sessionCount = SessionCount;

            totalBanner = TotalBanner;
            totalInterstitial = TotalInterstitial;
            totalRewarded = TotalRewarded;

            totalGameTime = TotalGameTime;
        }

        public EventProperty[] GetProperties(JuicySnapshotFlag flags)
        {
            List<EventProperty> properties = new List<EventProperty>();
            if ((flags & JuicySnapshotFlag.Version) != 0)
            {
                properties.Add(new EventProperty("juicysdk_version", CurrentInstallJuicyVersion));
                properties.Add(new EventProperty("first_juicysdk_version", FirstInstallJuicyVersion));
                properties.Add(new EventProperty("app_version", CurrentInstallAppVersion));
                properties.Add(new EventProperty("first_app_version", CurrentInstallAppVersion));
            }

            if ((flags & JuicySnapshotFlag.Game) != 0)
            {
                properties.Add(new EventProperty("level_index", levelIndex));
                properties.Add(new EventProperty("game_count", gameCount));
                properties.Add(new EventProperty("session_count", sessionCount));
            }

            if ((flags & JuicySnapshotFlag.Ads) != 0)
            { 
                properties.Add(new EventProperty("total_banner", totalBanner));
                properties.Add(new EventProperty("total_interstitial", totalInterstitial));
                properties.Add(new EventProperty("total_rewarded", totalRewarded));
            }
            if ((flags & JuicySnapshotFlag.Time) != 0)
            { 
                properties.Add(new EventProperty("total_game_time", totalGameTime));
            }

            return properties.ToArray();
        }

        public EventProperty[] GetDifferentialProperties(JuicySnapshot snapshot)
        {
            int bannerDiff = totalBanner - snapshot.totalBanner;
            int interstitialDiff = totalInterstitial - snapshot.totalInterstitial;
            int rewardedDiff = totalRewarded - snapshot.totalRewarded;

            EventProperty[] properties = new EventProperty[3];

            properties[0] = new EventProperty("game_banner", bannerDiff);
            properties[1] = new EventProperty("game_interstitial", interstitialDiff);
            properties[2] = new EventProperty("game_rewarded", rewardedDiff);

            return properties;
        }
    }
}