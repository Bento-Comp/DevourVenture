using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniUI
{
	[AddComponentMenu("UniUI/CanvasRendererSlave")]
	public class CanvasRendererSlave : MonoBehaviour
	{
		public CanvasRenderer master;

		CanvasRenderer slave;

		void Awake()
		{
			slave = GetComponent<CanvasRenderer>();
		}

		void LateUpdate()
		{
			UpdateLink();
		}

		void UpdateLink()
		{
			slave.SetColor(master.GetColor());
		}
	}
}