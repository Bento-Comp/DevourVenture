using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.SimpleGame
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/BlockColorManager")]
	public class BlockColorManager : MonoBehaviour
	{
		public bool useColor;

		public Color begin;

		public Color end;

		[SerializeField]
		float a = 1.0f;

		[SerializeField]
		float hBegin = 0.0f;

		[SerializeField]
		float hEnd = 180.0f;

		[SerializeField]
		float s = 210.0f;

		[SerializeField]
		float b = 210.0f;

		[SerializeField]
		int blockValueBegin = 1;

		[SerializeField]
		int blockValueEnd = 50;

		static BlockColorManager instance;

		public static BlockColorManager Instance
		{
			get
			{
				return instance;
			}
		}

		public int BlockValueBegin
		{
			get
			{
				return blockValueBegin;
			}
		}

		public int BlockValueEnd
		{
			get
			{
				return blockValueEnd;
			}
		}

		public Color GetBlockColorPercent(float blockValuePercent, bool inverse = false)
		{
			return GetBlockColor((int)(blockValuePercent * blockValueEnd), inverse);
		}

		public Color GetBlockColor(int blockValue, bool inverse = false)
		{
			float hPercent = Mathf.InverseLerp(blockValueBegin, blockValueEnd, blockValue);

			if(useColor)
			{
				HSBColor beginHSB;
				HSBColor endHSB;

				if(inverse)
				{
					beginHSB = HSBColor.FromColor(end);
					endHSB = HSBColor.FromColor(begin);
				}
				else
				{
					beginHSB = HSBColor.FromColor(begin);
					endHSB = HSBColor.FromColor(end);
				}

				HSBColor lerp;
				lerp.a = Mathf.Lerp(beginHSB.a, endHSB.a, hPercent);
				lerp.b = Mathf.Lerp(beginHSB.b, endHSB.b, hPercent);
				lerp.h = Mathf.Lerp(beginHSB.h, endHSB.h, hPercent);
				lerp.s = Mathf.Lerp(beginHSB.s, endHSB.s, hPercent);

				return lerp.ToColor();
			}
			else
			{
				float h = Mathf.Lerp(hBegin, hEnd, hPercent);

				HSBColor hsbColor;
				hsbColor.a = a;
				hsbColor.h = h / 360.0f;
				hsbColor.s = s / 255.0f;
				hsbColor.b = b / 255.0f;

				return hsbColor.ToColor();
			}
		}

		void Awake()
		{
			if(instance == null)
			{
				instance = this;
			}
			else
			{
				#if UNITY_EDITOR
				if(Application.isPlaying == false)
					return;
				#endif

				Debug.LogWarning("A singleton can only be instantiated once!");
				Destroy(gameObject);
				return;
			}
		}
		
		void OnDestroy()
		{
			if(instance == this)
			{
				instance = null;
			}
		}

		#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying == false)
			{
				instance = this;
			}
		}
		#endif
	}
}