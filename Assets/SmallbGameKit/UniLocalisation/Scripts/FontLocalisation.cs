using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniLocalisation
{
	[Serializable]
	public class LanguageFonts
	{
		public string languageCode;
		public FontLocalisationScheme fontLocalisationScheme;
		
		public List<LanguageFont> Fonts
		{
			get
			{
				return fontLocalisationScheme.fonts;
			}
		}
	}

	[Serializable]
	public class TextMeshFontParameters
	{
		public Font font;
		public float characterSize;
		
		public void CopyFrom(TextMesh a_rTextMesh)
		{
			font = a_rTextMesh.font;
			characterSize = a_rTextMesh.characterSize;
		}
	}

	[AddComponentMenu("UniLocalisation/Localisation/FontLocalisation")]
	public class FontLocalisation : MonoBehaviour
	{
		public string referenceLanguageCode = "en";
		
		public List<LanguageFonts> languageFonts;
		
		Dictionary<string, LanguageFonts> fontSchemeByLanguageCode = new Dictionary<string, LanguageFonts>();
		
		Dictionary<Font, LanguageFont> referenceFontToCurrentLanguageFont = new Dictionary<Font, LanguageFont>();
		
		static FontLocalisation instance;
		
		static public FontLocalisation Instance
		{
			get
			{
				return instance;
			}
		}
		
		public static void ApplyFontLocalisation(TextMesh a_rTextMesh, TextMeshFontParameters a_rInitialTextMeshFontParameters)
		{
			if(instance == null || Language.Instance == null)
			{
				return;
			}
			
			instance._ApplyFontLocalisation(a_rTextMesh, a_rInitialTextMeshFontParameters);
		}
		
		void Awake()
		{
			if(instance == null)
			{
				DontDestroyOnLoad(this);
				instance = this;
			}
			else
			{
				Destroy(gameObject);
				return;
			}
			
			FillDictionnary();
			Initialize();
		}
		
		void FillDictionnary()
		{
			fontSchemeByLanguageCode.Clear();
			foreach(LanguageFonts rLanguageFonts in languageFonts)
			{
				fontSchemeByLanguageCode.Add(rLanguageFonts.languageCode, rLanguageFonts);
			}
		}
		
		public void Initialize()
		{
			referenceFontToCurrentLanguageFont.Clear();
			string oReferenceLanguageCode = referenceLanguageCode;
			string eCurrentLanguageCode = Language.Instance.SelectedLanguageCode;
			
			LanguageFonts rReferenceLanguageFonts = fontSchemeByLanguageCode[oReferenceLanguageCode];
			
			LanguageFonts rCurrentLanguageFonts = null;
			if(fontSchemeByLanguageCode.TryGetValue(eCurrentLanguageCode, out rCurrentLanguageFonts) == false)
			{
				// Try to set a fallback language
				if(fontSchemeByLanguageCode.TryGetValue("fallback", out rCurrentLanguageFonts) == false)
				{
					rCurrentLanguageFonts = rReferenceLanguageFonts;
				}
			}
			
			int iFontIndex = 0;
			foreach(LanguageFont rReferenceFont in rReferenceLanguageFonts.Fonts)
			{
				LanguageFont rCurrentLanguageFont;
				if(iFontIndex < 0 || iFontIndex >= rCurrentLanguageFonts.Fonts.Count)
				{
					rCurrentLanguageFont = rReferenceFont;
				}
				else
				{
					rCurrentLanguageFont = rCurrentLanguageFonts.Fonts[iFontIndex];
				}
				
				referenceFontToCurrentLanguageFont.Add(rReferenceFont.font, rCurrentLanguageFont);
				
				++iFontIndex;
			}
		}
		
		void _ApplyFontLocalisation(TextMesh a_rTextMesh, TextMeshFontParameters a_rInitialTextMeshFontParameters)
		{
			LanguageFont rLanguageFont;
			if(referenceFontToCurrentLanguageFont.TryGetValue(a_rInitialTextMeshFontParameters.font, out rLanguageFont) == false)
			{
				return;
			}
			
			a_rTextMesh.font = rLanguageFont.font;
			a_rTextMesh.GetComponent<Renderer>().sharedMaterial = a_rTextMesh.font.material;
			a_rTextMesh.characterSize = a_rInitialTextMeshFontParameters.characterSize * (1.0f + rLanguageFont.characterScaleOffset);
			a_rTextMesh.fontStyle = rLanguageFont.fontStyle;
			TextMeshCase oTextMeshCase = UniEditor.ComponentBuilderUtility.GetOrAddComponent<TextMeshCase>(a_rTextMesh);
			oTextMeshCase.textCase = rLanguageFont.textCase;
		}
	}
}