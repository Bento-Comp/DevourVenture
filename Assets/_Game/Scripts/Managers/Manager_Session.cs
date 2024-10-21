using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(31999)]
public class Manager_Session : MonoBehaviour
{
    public static bool SessionActive { get; private set; }

    // Todo Sev : Start session is called from several places that only the patch
    // This is bug prone the session ought to be managed at onbly one place if possible
    public static void StartSession()
    {
        SessionActive = true;
    }

    public static void StopSession()
    {
        SessionActive = false;
    }
}
