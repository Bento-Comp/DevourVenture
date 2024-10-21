using UnityEngine;
using System.Collections;

namespace UniHapticFeedback
{
	[AddComponentMenu("UniHapticFeedback/HapticLoop")]
	public class HapticLoop : MonoBehaviour
	{
		public string loopName = "loop";

		public EHapticFeedbackType feedbackType = EHapticFeedbackType.SelectionChange;

		public float period = 0.1f;

		public bool ignoreIfNeedTo = true;

		float elapsedTime;

		public float SpeedMultiplicator { get; set; } = 1.0f;

        void OnEnable()
        {
			elapsedTime = 0.0f;    
        }

        void Update()
		{
			UpateLoop();
		}

		void UpateLoop()
		{
			elapsedTime += Time.deltaTime;

			if(elapsedTime * SpeedMultiplicator >= period)
			{
				elapsedTime = 0.0f;
				TriggerHapticFeedback();
			}
		}

		void TriggerHapticFeedback()
		{
			HapticFeedbackManager.TriggerHapticFeedback(feedbackType, ignoreIfNeedTo);
		}
	}
}