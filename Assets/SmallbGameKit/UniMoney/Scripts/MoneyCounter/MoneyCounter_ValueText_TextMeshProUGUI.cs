using UnityEngine;
using System.Collections;

using TMPro;

namespace UniMoney
{
	[AddComponentMenu("UniMoney/MoneyCounter_ValueText_TextMeshProUGUI")]
	public class MoneyCounter_ValueText_TextMeshProUGUI : MoneyCounter_ValueText_Base 
	{
		public TextMeshProUGUI text;

		protected override void SetText(string text)
		{
			this.text.text = text;
		}
	}
}