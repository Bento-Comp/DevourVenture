using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

using System.Text;

namespace UniLocalisation
{
	public static class Localisation
	{	
		public class LocalisedLanguage
		{
			string languageCode;

			Dictionary<string,string> stringById = new Dictionary<string, string>();

			public bool TryGetString(string stringId, out string localisedString)
			{
				return stringById.TryGetValue(stringId, out localisedString);
			}

			public LocalisedLanguage(string languageCode)
			{
				this.languageCode = languageCode;
				Load();
			}

			void Load()
			{
				stringById.Clear();

				string path = languageCode;

				TextAsset[] localisationTextAssets = Resources.LoadAll<TextAsset>(path);

				foreach(TextAsset localisationTextAsset in localisationTextAssets)
				{
					XmlDocument xmlDoc = new XmlDocument();
					try
					{
						xmlDoc.LoadXml(localisationTextAsset.text);
					}
					catch(System.Exception oException)
					{
						Debug.LogError("Error parsing : " + localisationTextAsset.name + "Msg : " + oException.Message);
					}

					XmlNodeList stringXmlNodeList = xmlDoc.GetElementsByTagName("String");
					foreach(XmlNode stringNode in stringXmlNodeList)
					{
						// Get or create the translations for this items
						string stringId = stringNode.Attributes["id"].Value;	
						string wantedTranslation = stringNode.InnerText;
						string translation;
						if(stringById.TryGetValue(stringId, out translation))
						{
							Debug.LogError("The translation for " + stringId + " in " + languageCode + " Already exist : " + translation + 
								"\nThe new translation : " + wantedTranslation + " will be ignored.");
						}
						else
						{
							stringById.Add(stringId, wantedTranslation);
						}
					}
				}
			}
		}

		static string defaultLanguageCode = "en-GB";
		//static string defaultLanguageCode = "fr-FR";

		static bool defaultLanguageCodeOverrided;
		static string overridedDefaultLanguageCode;

		static bool languageCodeSelected;
		static string selectedLanguageCode;

		static Dictionary<string, LocalisedLanguage> languageByLanguageCode = new Dictionary<string, LocalisedLanguage>();

		public static string DefaultLanguageCode
		{
			get
			{
				if(defaultLanguageCodeOverrided)
					return overridedDefaultLanguageCode;

				return defaultLanguageCode;
			}

			set
			{
				defaultLanguageCodeOverrided = true;
				overridedDefaultLanguageCode = value;
			}
		}

		public static string SelectedLanguageCode
		{
			get
			{
				if(languageCodeSelected)
					return selectedLanguageCode;

				return defaultLanguageCode;
			}

			set
			{
				languageCodeSelected = true;
				selectedLanguageCode = value;
			}
		}

		public static string GetString(string stringId)
		{
			return GetString(stringId, SelectedLanguageCode);
		}

		public static string GetString(string stringId, string languageCode)
		{
			return _GetOrientedString(stringId, languageCode);
		}

		static LocalisedLanguage CreateLanguage(string languageCode)
		{
			LocalisedLanguage language = new LocalisedLanguage(languageCode);
			languageByLanguageCode.Add(languageCode, language);

			return language;
		}
		
		static string _GetOrientedString(string stringID, string languageCode)
		{
			string oString = _GetString(stringID, languageCode);
			
			/*if(Language.Instance != null && Language.Instance.SelectedLanguage != null && Language.Instance.SelectedLanguage.flip)
			{
				oString = FlipString(oString);
			}*/
			
			return oString;
		}
		
		static string _GetString(string stringId, string languageCode)
		{
			LocalisedLanguage language = GetLanguage(languageCode);

			string localisedString;
			if(language.TryGetString(stringId, out localisedString))
			{	
				return localisedString;
			}
			else
			{
				if(languageCode != DefaultLanguageCode)
				{
					LocalisedLanguage defaultLanguage = GetDefaultLanguage();
					if(defaultLanguage.TryGetString(stringId, out localisedString))
					{	
						return localisedString;
					}
				}

				//Debug.LogWarning("Localised string not found : " + a_oStringId);
				return stringId;
			}
		}

		static LocalisedLanguage GetDefaultLanguage()
		{
			return GetLanguage(DefaultLanguageCode);
		}

		static LocalisedLanguage GetLanguage(string languageCode)
		{
			LocalisedLanguage language;
			if(languageByLanguageCode.TryGetValue(languageCode, out language) == false)
			{
				language = CreateLanguage(languageCode);
			}

			return language;
		}

		static string FlipString(string a_oStringToFlip)
		{
			string oLine = string.Empty;
			StringBuilder oFlippedStringBuilder = new StringBuilder();
			
			int iCharacterIndex = 0;
			foreach(char oCharacter in a_oStringToFlip)
			{	
				if(oCharacter == '\n')
				{
					if (oLine != string.Empty)
					{
						oFlippedStringBuilder.Append(Reverse(oLine));
					}
					
					oFlippedStringBuilder.Append(oCharacter);
					
					oLine = string.Empty;
				}
				else
				{
					oLine += oCharacter;
				}
				
				++iCharacterIndex;
			}
			
			// Last line
			if (oLine != string.Empty)
			{
				oFlippedStringBuilder.Append(Reverse(oLine));
			}
			
			return oFlippedStringBuilder.ToString();
		}
		
		static string Reverse(string a_oStringToReverse)
		{
			StringBuilder oReversedStringBuilder = new StringBuilder();
			foreach(char oCharacter in a_oStringToReverse)
			{
				oReversedStringBuilder.Insert(0, oCharacter);
			}
			
			return oReversedStringBuilder.ToString();
		}
	}
}