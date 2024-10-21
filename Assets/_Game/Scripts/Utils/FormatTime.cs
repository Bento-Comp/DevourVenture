using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormatTime 
{
    public static string Format_Time(int timeInSeconds)
    {
        int minutes = timeInSeconds / 60;
        int seconds = timeInSeconds % 60;

        string secondsString = "";

        if (seconds < 10)
            secondsString = "0" + seconds.ToString();
        else
            secondsString = seconds.ToString();

        return minutes.ToString() + ":" + secondsString;
    }
}
