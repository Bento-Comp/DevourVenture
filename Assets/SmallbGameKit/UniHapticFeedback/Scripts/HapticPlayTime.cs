using UnityEngine;
using System.Collections;

namespace UniHapticFeedback
{
	public class HapticPlayTime : MonoBehaviour
	{
		public bool playOnAwake = false;

		public bool loopCheck = true;

		public float waitTime = 1.0f;

		public float loopWaitTime = 0.0f;

		public EHapticFeedbackType feedbackType = EHapticFeedbackType.SelectionChange;

		private float loopTime = 0.0f;
		private bool flag = false;
		private float timer;

		void Update ()
		{
			loopTime -= Time.deltaTime;
			timer += Time.deltaTime;

			if((timer >= waitTime) && (flag == false) && (loopTime <= 0))
			{
				HapticFeedbackManager.TriggerHapticFeedback(feedbackType);

				flag = false;

				loopTime = loopWaitTime;

				if( loopCheck == false)
				{
					Destroy(this);
				}
			}
		}
	}
}