using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;

namespace UniMoney
{
	[AddComponentMenu("UniMoney/MoneyCounter_ValueText_UIText")]
	public class MoneyCounter_ValueText_UIText : MoneyCounter_ValueText_Base 
	{
		public Text text;

		protected override void SetText(string text)
		{
			this.text.text = text;
		}
	}
}