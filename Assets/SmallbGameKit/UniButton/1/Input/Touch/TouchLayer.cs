using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniButton
{
	[AddComponentMenu("UniButton/Input/Touch/TouchLayer")]
	public class TouchLayer : MonoBehaviour
	{
		Dictionary<string, int> touchLayerIDByName = new Dictionary<string, int>();
		
		List<string> touchLayerNames = new List<string>();
		
		static TouchLayer ms_oInstance;
		
		static public string TouchLayerIDToName(int a_iTouchLayerID)
		{
			return Instance._TouchLayerIDToName(a_iTouchLayerID);
		}
		
		static public int TouchLayerNameToID(string a_oTouchLayerName)
		{
			return Instance._TouchLayerNameToID(a_oTouchLayerName);
		}
		
		static public void AddLayerOnTop(string a_oTouchLayerName)
		{
			Instance._AddLayerOnTop(a_oTouchLayerName);
		}
		
		static public void RemoveLayer(string a_oTouchLayerName)
		{
			Instance._RemoveLayer(a_oTouchLayerName);
		}
		
		string _TouchLayerIDToName(int a_iTouchLayerID)
		{
			if(a_iTouchLayerID < 0 || a_iTouchLayerID >= touchLayerNames.Count)
			{
				return "";
			}
			else
			{
				return touchLayerNames[a_iTouchLayerID];
			}
		}
		
		int _TouchLayerNameToID(string a_oTouchLayerName)
		{
			int iLayerID;
			if(touchLayerIDByName.TryGetValue(a_oTouchLayerName, out iLayerID))
			{	
				return iLayerID;
			}
			else
			{
				return -1;
			}
		}
		
		void _AddLayerOnTop(string a_oTouchLayerName)
		{
			RemoveLayer(a_oTouchLayerName);
			
			touchLayerNames.Add(a_oTouchLayerName);
			touchLayerIDByName.Add(a_oTouchLayerName, touchLayerNames.Count - 1);
		}
		
		void _RemoveLayer(string a_oTouchLayerName)
		{
			int iLayerID = TouchLayerNameToID(a_oTouchLayerName);
			if(iLayerID != -1)
			{
				touchLayerIDByName.Remove(a_oTouchLayerName);
				touchLayerNames.RemoveAt(iLayerID);
			}
		}
		
		static TouchLayer Instance
		{
			get
			{
				if(ms_oInstance == null)
				{
					CreateInstance();
				}
				
				return ms_oInstance;
			}
		}
		
		void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		static void CreateInstance()
		{
			GameObject rInstanceGameObject = new GameObject(typeof(TouchLayer).Name);
			rInstanceGameObject.AddComponent<TouchLayer>();
		}
	}
}