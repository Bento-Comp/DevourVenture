using UnityEngine;
using System.Collections;

namespace UniApplication
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniApplication/SortingLayer/SpriteDepthSorting")]
	public class SpriteDepthSorting : MonoBehaviour 
	{
		public int sortingDepthOffset = 0;

		public bool useLocalZPosition = true;

		SpriteDepthSorting spriteDepthSortingParent;

		const float depthPrecision = 100.0f;
	
		public int SortingDepth
		{
			get
			{
				int sortingDepth;
				if(spriteDepthSortingParent == null)
				{
					sortingDepth = Mathf.RoundToInt(-transform.position.z * depthPrecision);
				}
				else
				{
					sortingDepth = spriteDepthSortingParent.SortingDepth;
					if(useLocalZPosition)
					{
						sortingDepth += Mathf.RoundToInt(-transform.localPosition.z * depthPrecision);
					}
				}

				sortingDepth += sortingDepthOffset;
						
				return sortingDepth;
			}
		}

		public void ForceSetParent(SpriteDepthSorting parent)
		{
			spriteDepthSortingParent = parent;
		}

		public void NotifyUpdateParent()
		{
			UpdateParent();
		}

		void Awake()
		{
			UpdateParent();
		}

		void Update()
		{
			Renderer sprite = GetComponent<Renderer>();
			if(sprite == null)
				return;

			if(Application.isPlaying == false)
				UpdateParent();

			sprite.sortingOrder = SortingDepth;
		}

		void UpdateParent()
		{
			Transform parentTransform = transform.parent;

			if(parentTransform == null)
			{
				spriteDepthSortingParent = null;
				return;
			}

			spriteDepthSortingParent = parentTransform.GetComponentInParent<SpriteDepthSorting>();
		}

		#if UNITY_EDITOR
		[UnityEditor.MenuItem("UniEditor/Sprite Depth Sorting/Add SpriteDepthSorting to all SpriteRenderers")]
		static void AddSpriteDepthSortingToAllSpriteRenderers()
		{
			foreach(SpriteRenderer sprite in GameObject.FindObjectsOfType<SpriteRenderer>())
			{
				if(sprite.GetComponent<SpriteDepthSorting>() == null)
				{
					sprite.gameObject.AddComponent<SpriteDepthSorting>();
				}
			}
		}
		#endif
	}
}
