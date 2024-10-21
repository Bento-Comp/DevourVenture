using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Juicy
{
	public class EventProperty
	{
		public string name;
		public object value;

		public EventProperty(string name, object value)
		{
			this.name = name;
			this.value = value;
		}

		public override string ToString()
		{
			return "[EventProperty] {" + name + "," + value + "}";
		}

		public static string AddToString(string prefix, EventProperty[] eventProperties)
		{
			string returnString = prefix;
			if(eventProperties != null)
			{
				foreach(EventProperty eventProperty in eventProperties)
				{
					returnString += " " + eventProperty;
				}
			}

			return returnString;
		}

		public static string AddToString(string prefix, List<EventProperty> eventProperties)
		{
			string returnString = prefix;
			if (eventProperties != null)
			{
				foreach (EventProperty eventProperty in eventProperties)
				{
					returnString += " " + eventProperty;
				}
			}

			return returnString;
		}

		public static float ExtractFloatValueFromArray(string name, float defaultValue, EventProperty[] properties)
        {
			float val = defaultValue;
			EventProperty prop = properties.FirstOrDefault(p => p.name == name);
			if (prop != null)
			{
				try
				{
					object converted = System.Convert.ChangeType(prop.value, typeof(float));
					val = (float)converted;
				}

				catch
				{
					val = defaultValue;
				}
			}

			return val;
		}
	}
}