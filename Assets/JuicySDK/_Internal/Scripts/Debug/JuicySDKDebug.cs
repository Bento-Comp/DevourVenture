using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Juicy;

namespace JuicyInternal
{
    [AddComponentMenu("JuicySDKInternal/JuicySDKDebug")]
    public class JuicySDKDebug : MonoBehaviour
    {
    	public float fontSize = 30.0f;

        public float top = 20.0f;
		public float left = 20.0f;

        public float screenHeightOfReference = 2048.0f;

        GUIStyle style = new GUIStyle();

        void OnGUI()
        {
            string text = "Juicy SDK " + JuicySDK.version + " debug";
            float scale = (float)Screen.height/(float)screenHeightOfReference;
			
			style.fontSize = (int)(fontSize * scale);

			style.normal.textColor = Color.gray;

			Rect safeArea = Screen.safeArea;

			GUI.Label(new Rect(safeArea.xMin + left * scale, (Screen.height - safeArea.yMax) + top * scale, 0.0f, 0.0f), text, style);
        }
    }
}