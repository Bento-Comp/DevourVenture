using UnityEngine;

namespace UniFillBar
{
	[AddComponentMenu("UniFillBar/FillContainer")]
	public class FillContainer : MonoBehaviour
	{
		public FillValueBase fill;

		public FillValueBase fill_trail;

		public void SetFill(float localFill, float globalFill, float localFillTrail, float globalFillTrail)
		{
			fill.SetFillAmount(localFill, globalFill);
			fill_trail.SetFillAmount(localFillTrail, globalFillTrail);
		}
	}
}