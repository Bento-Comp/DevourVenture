using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.UI;

namespace UniApplication
{
	[ExecuteInEditMode()]
	[AddComponentMenu("UniApplication/SortingLayer/SortingLayer")]
	public class SortingLayer : MonoBehaviour
	{
		public string sortingLayerName = "Default";

		public int orderInLayer = 0;

		void Awake()
		{
			UpdateSortingLayer();
		}

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying == false)
				UpdateSortingLayer();
		}
#endif

		void UpdateSortingLayer()
		{
			if(GetComponent<Renderer>() == null)
				return;
			
				GetComponent<Renderer>().sortingLayerName = sortingLayerName;
			
				GetComponent<Renderer>().sortingOrder = orderInLayer;
		}
	}
}