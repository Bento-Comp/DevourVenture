using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class HUDFps : MonoBehaviour 
{
	// Attach this to a GUIText to make a frames/second indicator.
	//
	// It calculates frames/second over each updateInterval,
	// so the display does not keep changing wildly.
	//
	// It is also fairly accurate at very low FPS counts (<10).
	// We do this not by simply counting frames per interval, but
	// by accumulating FPS for each frame. This way we end up with
	// correct overall FPS even if the interval renders something like
	// 5.5 frames.

	public  float updateInterval = 0.5F;

	float accum   = 0; // FPS accumulated over the interval
	int   frames  = 0; // Frames drawn over the interval
	float timeleft; // Left time for current interval

	UnityEngine.UI.Text textComponent;

	void Start()
	{
		textComponent = GetComponent<UnityEngine.UI.Text>();
		if( textComponent == null )
		{
			Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
			enabled = false;
			return;
		}
		timeleft = updateInterval;  
	}

	void Update()
	{
		if(Application.isPlaying == false)
		{
			UnityEngine.UI.Text textComponent = GetComponent<UnityEngine.UI.Text>();
			if(textComponent != null)
			{
				textComponent.text = "60 FPS";
			}
			return;
		}

		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;

		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0 )
		{
			// display two fractional digits (f2 format)
			float fps = accum/frames;
			string format = System.String.Format("{0:F2} FPS",fps);
			textComponent.text = format;

			//	DebugConsole.Log(format,level);
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
}