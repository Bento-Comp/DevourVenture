using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_ScreenSpaceCanvas : UniSingleton.Singleton<Manager_ScreenSpaceCanvas>
{
    public Canvas m_screenSpaceCanvas = null;

    private static Camera GetCanvasCamera(Canvas canvas)
    {
        return canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
    }

    public Camera GetScreenSpaceCamera()
    {
        return GetCanvasCamera(m_screenSpaceCanvas);
    }
}
