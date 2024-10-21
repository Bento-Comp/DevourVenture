using UnityEngine;
using System.Collections;

using UnityEngine.EventSystems;

using UnityEngine.UI;
using UniUI;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/LockedButton")]
	public class LockedButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
	{
		Animator animator;

		LockedBlock lockedBlock;

		public void OnPointerClick(PointerEventData eventData)
		{
			animator.Play("Hit");

			Vector3 cursorPosition = eventData.position;
			cursorPosition.z = eventData.pressEventCamera.WorldToScreenPoint(transform.position).z;
			lockedBlock.Hit(eventData.pressEventCamera.ScreenToWorldPoint(cursorPosition));
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			animator.enabled = true;
			animator.ResetTrigger("Normal");
			animator.ResetTrigger("Highlighted");
			animator.ResetTrigger("Disabled");
			animator.ResetTrigger("Pressed");
			animator.Play("Pressed");
		}

		void Awake()
		{
			animator = GetComponent<Animator>();

			lockedBlock = GetComponent<LockedBlock>();
		}
	}
}