using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml.Linq;

namespace UniUI
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniUI/ScaleToMatchInferiorRatio")]
	public class ScaleToMatchInferiorRatio : MonoBehaviour
	{
		public int referenceWidth = 9;
		public int referenceHeight = 16;

		public bool updateEachFrame;

		void Awake()
		{
			UpdateScale();
		}

		void Update()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				UpdateScale();
			}
			#endif
			{
				if(updateEachFrame == false)
					return;

				UpdateScale();
			}
		}

		void UpdateScale()
		{
			float widthScale = (float)Screen.width / (float)Screen.height * (float)referenceHeight / (float)referenceWidth;

			if(widthScale >= 1.0f)
				widthScale = 1.0f;

			transform.localScale = new Vector3(widthScale, widthScale, 1.0f);
		}
	}
}