using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UniLocalisation
{
	[Serializable]
	// Language Definition
	public class LanguageDefinition
	{
		public SystemLanguage systemLanguage;
		public string languageCode;
		public bool rightToLeft;
		public bool flip;
		
		// Constructor
		public LanguageDefinition(SystemLanguage a_eSystemLanguage, string a_oLanguageCode)
		{
			systemLanguage = a_eSystemLanguage;
			languageCode = a_oLanguageCode;
		}
	}

	[AddComponentMenu("UniLocalisation/Localisation/Language")]
	// Language
	public class Language : MonoBehaviour
	{
		// On language change
		public static Action onLanguageChange; 
		
		public bool overrideSystemLanguageInEditor = false;
		
		public SystemLanguage editorOverrideSystemLanguage = SystemLanguage.English;
		
		// The default system language
		public SystemLanguage defaultSystemLanguage = SystemLanguage.English;
		
		// The supported languages
		public List<LanguageDefinition> supportedLanguages = new List<LanguageDefinition>{new LanguageDefinition(SystemLanguage.English, "en")};
		
		LanguageDefinition selectedLanguage;
		
		// the selectect language
		string selectedLanguageCode;
		
		// the language code by system language
		Dictionary<SystemLanguage, LanguageDefinition> languageDefinitionBySystemLanguage = new Dictionary<SystemLanguage, LanguageDefinition>();

		// The selected language code save key
		static string mc_oSelectedLanguageCodeSaveKey = "SelectedLanguageCode";
		
		// Undefined language
		static string mc_oUndefinedLanguageCode = "Undefined Language Code";
		
		// The instance
		static Language ms_oInstance;
		
		// Default Language Code
		public string DefaultLanguageCode
		{
			get
			{
				return languageDefinitionBySystemLanguage[defaultSystemLanguage].languageCode;
			}
		}

		public string InitialLanguageCode
		{
			get
			{
				SystemLanguage eSystemLanguage = Application.systemLanguage;

				if(overrideSystemLanguageInEditor && Application.isEditor)
				{
					eSystemLanguage = editorOverrideSystemLanguage;
				}

				LanguageDefinition oLanguage;
				if(languageDefinitionBySystemLanguage.TryGetValue(eSystemLanguage, out oLanguage) == false)
				{
					oLanguage = languageDefinitionBySystemLanguage[defaultSystemLanguage];
				}

				return oLanguage.languageCode; 
			}
		}
		
		// Get the selected language
		public string SelectedLanguageCode
		{
			get
			{
				return _SelectedLanguageCode;
			}
			
			set
			{
				PlayerPrefs.SetString(mc_oSelectedLanguageCodeSaveKey, selectedLanguageCode);
					
				_SelectedLanguageCode = value;
			}
		}
		
		string _SelectedLanguageCode
		{
			get
			{
				return selectedLanguageCode;
			}
			
			set
			{
				if(selectedLanguageCode != value)
				{
					selectedLanguageCode = value;
					OnLanguageChange();
					if(onLanguageChange != null)
					{
						onLanguageChange();
					}
				}
			}
		}
		
		// Set Language Code
		// Called from html
		public void SetLanguageCodeFromHTML(string a_oLanguageCode)
		{
			SelectedLanguageCode = a_oLanguageCode;
		}
		
		// Instance accessor
		static public Language Instance
		{
			get
			{
				return ms_oInstance;
			}
		}
		
		// Awake
		void Awake()
		{
			if(ms_oInstance == null)
			{
				ms_oInstance = this;
			}
			else
			{
				Destroy(gameObject);
				return;
			}
			
			FillDictionnary();
			Initialize();
			
	#if UNITY_WEBPLAYER
			Application.ExternalCall("SetApplicationLanguage");
	#endif
		}
		
		// Fill Dictionnary
		void FillDictionnary()
		{
			languageDefinitionBySystemLanguage.Clear();
			foreach(LanguageDefinition rLanguageDefinition in supportedLanguages)
			{
				languageDefinitionBySystemLanguage.Add(rLanguageDefinition.systemLanguage, rLanguageDefinition);
			}
		}
		
		// Initialize
		void Initialize()
		{
			Localisation.DefaultLanguageCode = DefaultLanguageCode;
			string oSavedLanguage = PlayerPrefs.GetString(mc_oSelectedLanguageCodeSaveKey, mc_oUndefinedLanguageCode);
			if(oSavedLanguage == mc_oUndefinedLanguageCode)
			{
				_SelectedLanguageCode = InitialLanguageCode;
			}
			else
			{
				_SelectedLanguageCode = oSavedLanguage;
			}
		}
		
		void OnLanguageChange()
		{
			Localisation.SelectedLanguageCode = _SelectedLanguageCode;
		}
	}
}