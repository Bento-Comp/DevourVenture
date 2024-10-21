using UnityEngine;
using System.Collections;

using UnityEngine.UI;

namespace UniUI
{
	[ExecuteAlways]
	[AddComponentMenu("UniUI/TextSlave")]
	public class TextSlave : MonoBehaviour
	{
		public Text master;

		Text slave;

		Text Slave
		{
			get
			{
				#if UNITY_EDITOR
				if(Application.isPlaying)
				{
					if(slave == null)
						GetSlave();
				}
				#endif

				return slave;
			}
		}

		void Awake()
		{
			GetSlave();
		}

		void LateUpdate()
		{
			#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(master == null)
					return;

				if(slave == null)
					GetSlave();

				if(slave == null)
					return;
			}
			#endif

			UpdateLink();
		}

		void GetSlave()
		{
			slave = GetComponent<Text>();
		}

		void UpdateLink()
		{
			Slave.text = master.text;
		}
	}
}