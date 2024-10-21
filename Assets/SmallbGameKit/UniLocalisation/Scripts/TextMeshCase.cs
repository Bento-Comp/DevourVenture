using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace UniLocalisation
{
	[AddComponentMenu("UniLocalisation/TextMeshCase")]
	public class TextMeshCase : MonoBehaviour
	{
		public ETextCase textCase;
		
		TextMesh textMesh;
		
		public void ApplyConstraint()
		{
			switch(textCase)
			{
			case ETextCase.UpperCase:
			{ 
				textMesh.text = textMesh.text.ToUpper();
			}
				break;
				
			case ETextCase.LowerCase:
			{ 
				textMesh.text = textMesh.text.ToLower();
			}
				break;
			}
		}
		
		void Awake()
		{
			textMesh = GetComponent<TextMesh>();
		}
		
		void Start()
		{
			ApplyConstraint();
		}
		
		void LateUpdate()
		{
			ApplyConstraint();
		}
	}
}