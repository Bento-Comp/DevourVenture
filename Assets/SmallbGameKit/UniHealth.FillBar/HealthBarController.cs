using UnityEngine;

using UniFillBar;

namespace UniHealth
{
	[DefaultExecutionOrder(-1)]
	[AddComponentMenu("UniHealth/HealthBarController")]
	public class HealthBarController : MonoBehaviour
	{
		public Health health;

		public FillBarController fillBarController;

		float CurrentHealthPercent => health.CurrentHealth/health.MaxHealth;

		void Awake()
		{
			if(health.Initialized)
			{
				OnInitializeHealth();
			}
			else
			{
				health.onInitializeHealth += OnInitializeHealth;
			}
			
			health.onGiveHealth += OnGiveHealth;
			health.onRemoveHealth += OnRemoveHealth;
		}

		void OnDestroy()
		{
			health.onInitializeHealth -= OnInitializeHealth;
			health.onGiveHealth -= OnGiveHealth;
			health.onRemoveHealth -= OnRemoveHealth;
		}

		void OnInitializeHealth()
		{
			fillBarController.Initialize(1.0f, CurrentHealthPercent);
		}

		void OnGiveHealth()
		{
			fillBarController.SetFill(CurrentHealthPercent);
		}

		void OnRemoveHealth()
		{
			fillBarController.SetFill(CurrentHealthPercent);
		}
	}
}