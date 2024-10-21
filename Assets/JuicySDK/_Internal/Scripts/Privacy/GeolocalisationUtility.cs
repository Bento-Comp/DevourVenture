/*using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace JuicyInternal
{
	// http://ip-api.com/docs/api:json
	[System.Serializable]
	public class GeolocalisationData
	{
		const string status_success = "success";

		public bool Success
		{
			get
			{
				return status == status_success;
			}
		}

		[SerializeField]
		string status = "";
	
		public string country;
		public string countryCode;
		public string region;
		public string regionName;
		public string city;
		public string zip;
		public string lat;
		public string lon;
		public string timezone;
		public string isp;
		public string org;
		//public string as;
		public string query;

		public override string ToString()
		{
			return "Success = " + Success
				+ " | country = " + country
				+ " | countryCode = " + countryCode
				+ " | region = " + region
				+ " | regionName = " + regionName
				+ " | city = " + city
				+ " | zip = " + zip
				+ " | lat = " + lat
				+ " | lon = " + lon
				+ " | timezone = " + timezone
				+ " | isp = " + isp
				+ " | org = " + org
				+ " | query = " + query;
		}
	}

	public static class GeolocalisationUtility
	{
		public static void FetchGeolocalisationData(MonoBehaviour askingBehaviour, System.Action<GeolocalisationData> onFetchSuccess, System.Action onFetchFail)
		{
			askingBehaviour.StartCoroutine(GetGeolocalisationData(onFetchSuccess, onFetchFail));
		}

		static IEnumerator GetGeolocalisationData(System.Action<GeolocalisationData> onFetchSuccess, System.Action onFetchFail)
		{
			GeolocalisationData geolocalisationData = null;

			UnityWebRequest webRequest = UnityWebRequest.Get("http://ip-api.com/json");
		
			yield return webRequest.SendWebRequest();

			if(webRequest.isNetworkError)
			{
				JuicySDKLog.Verbose("GetGeolocalisationData : Network Error : error = " + webRequest.error);
			}
			else
			{		
				string json = webRequest.downloadHandler.text;
		
				JuicySDKLog.Verbose("GetGeolocalisationData : Received Json : json = " + json);

				geolocalisationData = JsonUtility.FromJson<GeolocalisationData>(json);
		
				if(geolocalisationData == null)
				{	
					JuicySDKLog.Verbose("GetGeolocalisationData : Could not get geo data : json = " + json.ToString());
				}
				else
				{
					JuicySDKLog.Verbose("GetGeolocalisationData : Geolocalisation data request : geolocalisationData = " + geolocalisationData);

					if(geolocalisationData.Success == false)
					{
						JuicySDKLog.Verbose("GetGeolocalisationData : Geolocalisation data request fail : geolocalisationData = " + geolocalisationData);
					}
				}
			}

			if(geolocalisationData != null && geolocalisationData.Success)
			{
				if(onFetchSuccess != null)
					onFetchSuccess(geolocalisationData);
			}
			else
			{
				if(onFetchFail != null)
					onFetchFail();
			}
		}
	}
}*/