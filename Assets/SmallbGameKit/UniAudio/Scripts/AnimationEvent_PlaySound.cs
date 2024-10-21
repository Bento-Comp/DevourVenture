using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniAudio
{
	[AddComponentMenu("UniAudio/AnimationEvent_PlaySound")]
	public class AnimationEvent_PlaySound : MonoBehaviour
	{
		public enum EPlayFilter
		{
			None,
			DontPlayInReverse,
			PlayOnlyInReverse
		}

		public bool playSound;

		public SoundProperty sound;

		public EPlayFilter playFilter;

		public Animator animator;

		bool soundPlayed = false;

		bool CanPlay
		{
			get
			{
				switch(playFilter)
				{
					default:
					case EPlayFilter.None:
						return true;

					case EPlayFilter.DontPlayInReverse:
					{
						return animator.GetCurrentAnimatorStateInfo(0).speedMultiplier * animator.GetCurrentAnimatorStateInfo(0).speed >= 0.0f;
					}

					case EPlayFilter.PlayOnlyInReverse:
					{
						return animator.GetCurrentAnimatorStateInfo(0).speedMultiplier * animator.GetCurrentAnimatorStateInfo(0).speed <= 0.0f;
					}
				}
			}
		}

		public void PlaySoundByAsset(Object soundAssetObject)
		{
			if(CanPlay == false)
				return;
			
			SoundAsset soundAsset = soundAssetObject as SoundAsset;
			if(soundAsset == null)
				return;

			AudioManager.Instance.PlaySound(soundAsset);
		}

		public void PlaySoundByName(string soundName)
		{
			if(CanPlay == false)
				return;

			AudioManager.Instance.PlaySound(soundName);
		}

		public void PlaySound()
		{
			if(CanPlay == false)
				return;
			
			AudioManager.Instance.PlaySound(sound);
		}

		void OnEnable()
		{
			soundPlayed = false;
			playSound = false;
		}

		void Awake()
		{
			if(animator == null)
				animator = GetComponent<Animator>();
		}

		void Update()
		{
			if(playSound)
			{
				if(soundPlayed == false)
				{
					if(CanPlay)
					{
						soundPlayed = true;

						AudioManager.Instance.PlaySound(sound);
					}
				}
			}
			else
			{
				soundPlayed = false;
			}
		}
	}
}