using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniHealth
{
	[AddComponentMenu("UniHealth/Health")]
	public class Health : MonoBehaviour
	{
		public System.Action onInitializeHealth;

		public System.Action onRemoveHealth;

		public System.Action onGiveHealth;

		public System.Action onNoMoreHealth;

		public bool invincible;

		public bool autoInitialize = true;

		public float startingHealth = 3;

		public float startingMaxHealth = 3;

		public float damageTakenMultiplicator = 1.0f;

		float currentHealth;

		float maxHealth;

		bool initialized;

		float lastHealthLoss;

		public bool Initialized
		{
			get
			{
				return initialized;
			}
		}

		public float CurrentHealth
		{
			get
			{
				return currentHealth;
			}
		}

		public float LostHealth
		{
			get
			{
				return maxHealth - currentHealth;
			}
		}

		public float MaxHealth
		{
			get
			{
				return maxHealth;
			}
		}

		public float LastHealthLoss
		{
			get
			{
				return lastHealthLoss;
			}
		}

		public bool NoMoreHealth => CurrentHealth <= 0.0f;

		bool Invincible
		{
			get
			{
				return invincible;
			}
		}

		public void InitializeHealth(float startingHealth, float startingMaxHealth)
		{
			maxHealth = startingMaxHealth;
			currentHealth = startingHealth;

			onInitializeHealth?.Invoke();

			initialized = true;
		}

		public void InitializeHealth(float startingHealth)
		{
			InitializeHealth(startingHealth, startingHealth);
		}

		public void ForceNoMoreHealth(bool ignoreInvincibility = false)
		{
			if(ignoreInvincibility == false)
			{
				if(Invincible)
					return;
			}

			OnNoMoreHealth();
		}

		public void GiveHealth(float healthToGive)
		{
			if(isActiveAndEnabled == false)
				return;

			Debug.Log("GiveHealth : currentHealth = " + currentHealth + " | healthToGive = " + healthToGive);

			currentHealth += healthToGive;

			if(currentHealth > maxHealth)
				currentHealth = maxHealth;

			onGiveHealth?.Invoke();  
		}

		public void RemoveHealth(float healthToRemove)
		{
			//Debug.Log("healthToRemove = " + healthToRemove);
			if(isActiveAndEnabled == false)
				return;

			if(Invincible)
				return;

			healthToRemove *= damageTakenMultiplicator;

			lastHealthLoss = healthToRemove;

			currentHealth -= healthToRemove;

			if(currentHealth <= 0.0f)
			{
				OnNoMoreHealth();
			}

			onRemoveHealth?.Invoke();
		}

		void Start()
		{
			if(autoInitialize)
			{
				InitializeHealth(startingHealth, startingMaxHealth);
			}
		}

		void OnNoMoreHealth()
		{
			currentHealth = 0.0f;

			onNoMoreHealth?.Invoke();
		}
	}
}
