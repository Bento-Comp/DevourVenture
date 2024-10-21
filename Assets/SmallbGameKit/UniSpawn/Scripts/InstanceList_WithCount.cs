using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniSpawn
{
	[DefaultExecutionOrder(-32000)]
	[ExecuteAlways]
	[AddComponentMenu("UniSpawn/InstanceList")]
	public class InstanceList_WithCount<InstanceType> : InstanceList<InstanceType>, ISetCountInstanceList where InstanceType : Component
	{
		[SerializeField]
		int count = 3;

		public override int Count
		{
			get => count;
		}

		public void SetCount(int value)
		{
			if(count == value)
				return;

			count = value;

			UpdateList();
		}
	}
}
