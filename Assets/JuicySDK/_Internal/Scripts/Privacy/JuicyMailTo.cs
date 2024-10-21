using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JuicyInternal
{
    public class JuicyMailTo : MonoBehaviour
    {
        public static void SendMail(string mailAdress, string mailSubject, string mailBody)
        {
            string mailToURL = "mailto:" +  mailAdress;
            mailToURL += "?subject=" + EscapeUriString(mailSubject);
            mailToURL += "&body=" + EscapeUriString(mailBody);
            Debug.Log(mailToURL);
            Application.OpenURL(mailToURL);
        }

        static string EscapeUriString(string text)
        {
            return System.Uri.EscapeUriString(text);
        }
    }
}
