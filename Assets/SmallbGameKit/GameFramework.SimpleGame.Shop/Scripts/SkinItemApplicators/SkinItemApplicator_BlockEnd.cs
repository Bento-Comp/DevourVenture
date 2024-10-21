using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniSkin;

namespace GameFramework.SimpleGame
{
	[AddComponentMenu("GameFramework/SimpleGame/SkinItemApplicator_BlockEnd")]
	public class SkinItemApplicator_BlockEnd : SkinItemApplicator_ColorBase
	{
		BlockColorManager blockColorManager;

		protected override Color CurrentColor
		{
			get
			{
				if(blockColorManager == null)
					blockColorManager = GetComponent<BlockColorManager>();
			
				return blockColorManager.end;
			}
		}

		protected override void OnColorChange(Color color)
		{
			if(blockColorManager == null)
				blockColorManager = GetComponent<BlockColorManager>();
			
			blockColorManager.end = color;
		}
	}
}
