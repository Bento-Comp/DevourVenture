using UnityEngine;

namespace UniFillBar
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniFillBar/FillValueVisual")]
	public class FillValueVisualBase : MonoBehaviour
	{
		public FillValueBase fillValue;

		public GameObject activationRoot;

		public bool useGlobalFillAmount;

		public bool deactivateWhenEmpty = true;

		public bool deactivateWhenFull;

		bool Activate
		{
			set
			{
				activationRoot.SetActive(value);
			}
		}

		protected virtual void OnSetFillAmount(float fillAmount)
		{
		}

		protected virtual void OnFillAmountChange(float fillAmountChange)
		{
		}

		void OnEnable()
		{
			#if UNITY_EDITOR
			if(fillValue == null)
				return;
			#endif

			fillValue.onFillAmountChange += OnFillAmountChange;
			SetFillAmount(fillValue.FillAmount, fillValue.GlobalFillAmount);
		}

		void OnDisable()
		{
			#if UNITY_EDITOR
			if(fillValue == null)
				return;
			#endif

			fillValue.onFillAmountChange -= OnFillAmountChange;
		}

		/*void Start()
		{
			OnFillAmountChange();
		}*/

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			if(fillValue == null)
				return;

			SetFillAmount(fillValue.FillAmount, fillValue.GlobalFillAmount);
		}
#endif

		void OnFillAmountChange(float localFillAmount, float globalFillAmount)
		{
			SetFillAmount(localFillAmount, globalFillAmount);
		}

		void SetFillAmount(float localFillAmount, float globalFillAmount)
		{
			float fillAmount = useGlobalFillAmount ? globalFillAmount : localFillAmount;
			OnSetFillAmount(fillAmount);

			if(activationRoot != null)
			{
				if(deactivateWhenEmpty && fillAmount <= 0.0f)
				{
					Activate = false;
				}
				else if(deactivateWhenFull && fillAmount >= 1.0f)
				{
					Activate = false;
				}
				else
				{
					Activate = true;
				}
			}
		}
	}
}