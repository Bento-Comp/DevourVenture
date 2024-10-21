using UnityEngine;
using System.Collections.Generic;

namespace UniFillBar
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniFillBar/FillBar")]
	public class FillBar : MonoBehaviour
	{
		public System.Action onUpdateFillContainer;

		[SerializeField]
		int itemCount = 1;

		[SerializeField]
		FillValueBase fill;

		[SerializeField]
		FillValueBase fillTrail;

		[SerializeField]
		Transform itemPivot = null;

		[SerializeField]
		FillContainer itemPrefab = null;

		[SerializeField]
        List<FillContainer> items = new List<FillContainer>();

		List<FillContainer> Items
		{
			get
			{
				return items;
			}
		}

		public int ItemCount
		{
			set
			{
				if(itemCount != value)
				{
					itemCount = value;
					UpdateList();
				}
			}

			get
			{
				return itemCount;
			}
		}

		public void RuntimeInitialize(int itemCount, float initialFill = 1.0f)
		{
			this.itemCount = itemCount;
			fill.SetFillAmount(initialFill);
			fillTrail.SetFillAmount(initialFill);

			UpdateList();
		}

		public void RuntimeUpdate(float fill, float fill_hit)
		{
			this.fill.SetFillAmount(fill);
			this.fillTrail.SetFillAmount(fill_hit);

			UpdateFill();
		}

		#if UNITY_EDITOR
		void Update()
		{
			if(Application.isPlaying)
				return;

			if(fill == null || fillTrail == null)
				return;


			UpdateList();
		}
		#endif

		void ClearList()
        {
            foreach(FillContainer item in items)
            {
                DestroyImmediate(item.gameObject);
            }
            items.Clear();
        }

		void UpdateFill()
        {
            int index = 0;
            foreach(FillContainer item in items)
            {
				if(item == null)
					continue;

                ApplyItemParameters(item, index);

                ++index;
            }
        }

        void UpdateList()
        {
            int index = 0;
            for(int i = 0; i < itemCount; ++i)
            {
                if(index >= items.Count)
                {
                    items.Add(null);
                }

                FillContainer item = items[index];

                if(item == null)
                {
                    item = CreateItem();
                    items[index] = item;
                }

                ApplyItemParameters(item, index);

                ++index;
            }

            while(items.Count > index)
            {
                int indexToRemove = items.Count - 1;

                FillContainer itemToRemove = items[indexToRemove];

                if(itemToRemove != null)
                    DestroyImmediate(itemToRemove.gameObject);

                items.RemoveAt(indexToRemove);
            }

			onUpdateFillContainer?.Invoke();
        }

        FillContainer CreateItem()
        {
			FillContainer item;

			Transform pivot = itemPivot;
			if(pivot == null)
				pivot = transform;

			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				item = UnityEditor.PrefabUtility.InstantiatePrefab(itemPrefab, pivot) as FillContainer;
			}
			else
			#endif
			{
				item = Instantiate(itemPrefab, pivot, false);
			}

            return item;
        }

        void ApplyItemParameters(FillContainer item, int itemIndex)
        {
			if(item == null)
				return;

            item.name = itemPrefab.name + " (" + (itemIndex + 1) + ")";

			item.SetFill(ComputeLocalFill(itemIndex, fill.FillAmount), fill.FillAmount,
			ComputeLocalFill(itemIndex, fillTrail.FillAmount), fillTrail.FillAmount);
        }

		float ComputeLocalFill(int itemIndex, float globalFill)
        {
			if(itemCount <= 0)
				return 1.0f;

			float itemLength = 1.0f/itemCount;

			float item_globalFillMin = itemIndex * itemLength;
			float item_globalFillMax = item_globalFillMin + itemLength;

			float localFill = Mathf.InverseLerp(item_globalFillMin, item_globalFillMax, globalFill);

			return localFill;
		}
	}
}