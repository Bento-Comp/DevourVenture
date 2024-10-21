using UnityEngine;

namespace UniFillBar
{
	[AddComponentMenu("UniFillBar/FillValueBase")]
	public abstract class FillValueBase : MonoBehaviour
	{
		public System.Action<float, float> onFillAmountChange;

		public abstract float FillAmount {get; protected set;}

		public abstract float GlobalFillAmount {get; protected set;}

		public void SetFillAmount(float newFillAmount)
		{
			SetFillAmount(newFillAmount, newFillAmount);
		}

		public void SetFillAmount(float newFillAmount, float globalFillAmount)
		{
			FillAmount = newFillAmount;

			GlobalFillAmount = globalFillAmount;

			OnFillAmountChange();
		}

		protected void OnFillAmountChange()
		{
			onFillAmountChange?.Invoke(FillAmount, GlobalFillAmount);
		}
	}
}