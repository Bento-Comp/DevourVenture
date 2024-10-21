using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UniButton
{
	public static class InputLayer
	{
		static HashSet<string> ms_oInactiveInputLayerNames = new HashSet<string>();
		
		static public void Activate(string a_rInputLayerName, bool a_bActive)
		{
			if(a_bActive)
			{
				ms_oInactiveInputLayerNames.Remove(a_rInputLayerName);
			}
			else
			{
				ms_oInactiveInputLayerNames.Add(a_rInputLayerName);
			}
		}
		
		static public bool IsActive(string a_rInputLayerName)
		{
			return ms_oInactiveInputLayerNames.Contains(a_rInputLayerName) == false;
		}
	}
}