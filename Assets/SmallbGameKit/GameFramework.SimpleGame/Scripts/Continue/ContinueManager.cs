using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/ContinueManager")]
	public class ContinueManager : GameBehaviour 
	{
		static public System.Action onContinue;

		static public System.Action<bool> onContinueCountdownToggle;

		static public System.Action onGameOverConfirmed;

		public int maxContinueByPlay = 1;

		public float continueCountdownDuration = 5.0f;

		bool continueCountdownInProgress;

		float continueCountdownRemainingTime;

		int continueUsedThisPlay;

        bool waitForContinueCanBeCalledConfirmation;

		bool continueOpportunityInProgress;

		static ContinueManager instance;

		static public ContinueManager Instance
		{
			get
			{
				return instance;
			}
		}

		public float CountdownRemainingTime
		{
			get
			{
				return continueCountdownRemainingTime;
			}
		}

        public bool CountdownInProgress
        {
            get
            {
                return continueCountdownInProgress;
            }
        }

        public void NotifyGameOverConfirmed()
		{
			OnGameOverConfirmed();
		}

		public void NotifyContinueOpportunity()
		{
			if(continueUsedThisPlay >= maxContinueByPlay)
				return;

            waitForContinueCanBeCalledConfirmation = true;

			StartContinueOpportunity();
		}

		public void AskForContinue()
		{
			if(continueCountdownInProgress == false)
				return;

			StopCountdown();

			OnAskForContinue();
		}

		protected virtual void NotifyContinueOpportunityStart(System.Action<bool> onContinueCanBeCalled)
		{
		}

		protected virtual void NotifyContinueOpportunityEnd()
		{
		}
			
		protected virtual void OnAskForContinue()
		{
			NotifyAskForContinueAnswer(true);
		}

		protected void NotifyAskForContinueAnswer(bool canContinue)
		{
			if(canContinue)
			{
				Continue();
			}
			else
			{
				OnGameOverConfirmed();
			}
		}

		protected override void OnLevelCompleted(bool success)
		{
			base.OnLevelCompleted(success);

			continueCountdownInProgress = false;

			OnContinueCountdownToggle();
		}

		protected override void OnLoadGame()
		{
			StopCountdown();
			continueUsedThisPlay = 0;
			continueCountdownInProgress = false;
		}

		protected override void OnLoadGameEnd(bool reloadSceneAfter)
		{
			if(continueCountdownInProgress)
			{
				StopCountdown();
				OnGameOverConfirmed();
			}
		}

		protected override void OnAwake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}

		protected override void OnAwakeEnd()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		void Update()
		{
			if(continueCountdownInProgress)
			{
				continueCountdownRemainingTime -= Time.deltaTime;
				if(continueCountdownRemainingTime <= 0.0f)
				{
					StopCountdown();
					OnGameOverConfirmed();
				}
			}
		}

		void OnContinueCanBeCalled(bool canContinue)
		{
            if (waitForContinueCanBeCalledConfirmation == false)
                return;

            waitForContinueCanBeCalledConfirmation = false;

            if (canContinue)
			{
				continueCountdownInProgress = true;
				continueCountdownRemainingTime = continueCountdownDuration;
				OnContinueCountdownToggle();
			}
			else
			{
				OnGameOverConfirmed();
			}
		}

		void StopCountdown()
		{
			if(continueCountdownInProgress == false)
				return;
			
			continueCountdownInProgress = false;
			OnContinueCountdownToggle();

			StopContinueOpportunity();
		}

		void OnContinueCountdownToggle()
		{
			if(onContinueCountdownToggle != null)
			{
				onContinueCountdownToggle(continueCountdownInProgress);
			}
		}

		void Continue()
		{
			++continueUsedThisPlay;

			continueCountdownInProgress = false;

			OnContinueCountdownToggle();

			Game.Instance.CancelGameOver();

			Debug.Log("Continue");
			if(onContinue != null)
			{
				onContinue();
			}
		}

		void OnGameOverConfirmed()
		{
			if(onGameOverConfirmed != null)
			{
				onGameOverConfirmed();
			}

			StopContinueOpportunity();
		}

		void StartContinueOpportunity()
		{
			continueOpportunityInProgress = true;

            NotifyContinueOpportunityStart(OnContinueCanBeCalled);
		}

		void StopContinueOpportunity()
		{
			if(continueOpportunityInProgress == false)
				return;

			continueOpportunityInProgress = false;
			NotifyContinueOpportunityEnd();
		}
	}
}
