using UnityEngine;

namespace UniFillBar
{
	[DefaultExecutionOrder(-1)]
	[AddComponentMenu("UniFillBar/FillBarController")]
	public class FillBarController : MonoBehaviour
	{
		public System.Action onFillBarFillChange;

		public FillBar fillBar;

		public bool smoothTimeInPercent = false;

		public float removeSmoothTime = 0.1f;

		public float addSmoothTime = 0.1f;

		public float reachTargetEpsilon = 0.01f;

		float maxFill;

		float targetFill;

		float currentFill;

		bool removeFillChangeInProgress;

		bool addFillChangeInProgress;

		float currentVelocity = 0.0f;

		public bool Full => targetFill >= maxFill;

		public void Initialize(float maxFill, float currentFill)
		{
			this.maxFill = maxFill;
		 	this.currentFill = currentFill;
			this.targetFill = currentFill;
			fillBar.RuntimeInitialize((int)maxFill, currentFill/maxFill);
		}

		public void ChangeFillCapacity(float maxFill)
		{
			float fillPercent = currentFill/maxFill;

			this.maxFill = maxFill;
		 	this.currentFill = fillPercent * this.maxFill;

			fillBar.RuntimeInitialize((int)maxFill, fillPercent);
		}

		public void SetFill(float fill)
		{
			targetFill = fill;
			if(targetFill < currentFill)
			{
				addFillChangeInProgress = false;
				removeFillChangeInProgress = true;
			}
			else
			{
				addFillChangeInProgress = true;
				removeFillChangeInProgress = false;
			}

			UpdateFillBarFill();

			OnFillBarFillChange();
		}

		void Update()
		{
			UpdateFillRemoval();
		}

		void UpdateFillRemoval()
		{
			if(addFillChangeInProgress || removeFillChangeInProgress)
			{
				float smoothTime = removeFillChangeInProgress?removeSmoothTime:addSmoothTime;
				if(smoothTimeInPercent)
				{
					smoothTime *= maxFill;
				}

				currentFill = Mathf.SmoothDamp(currentFill, targetFill, ref currentVelocity, smoothTime);

				if(Mathf.Abs(currentFill - targetFill) <= reachTargetEpsilon)
				{
					removeFillChangeInProgress = false;
					addFillChangeInProgress = false;
					currentFill = targetFill;
				}
			}

			UpdateFillBarFill();
		}

		void OnFillBarFillChange()
		{
			onFillBarFillChange?.Invoke();
		}

		void UpdateFillBarFill()
		{
			float fill;
			float fill_hit;

			if(addFillChangeInProgress)
			{
				fill_hit = targetFill/maxFill;
				fill = currentFill/maxFill;
			}
			else if(removeFillChangeInProgress)
			{
				fill = targetFill/maxFill;
				fill_hit = currentFill/maxFill;
			}
			else
			{
				fill = targetFill/maxFill;
				fill_hit = fill;
			}


			fillBar.RuntimeUpdate(fill, fill_hit);
		}
	}
}