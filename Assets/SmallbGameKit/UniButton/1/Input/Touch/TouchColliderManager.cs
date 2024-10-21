using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;

namespace UniButton
{
	// Save and Reuse physical raycast checks
	// Primarly created to help optimise TouchZone_Collider touch tests
	// => Avoid making an other identical raycast check for just an other touch zone
	// Use the fact that natively two rays created with the same camera and mouse position
	// are equals
    // => the user don't have to use the same Ray structure so the query don't have to initiate only from one central place
	// that allow for a decentralise and flexible checks and the overhead is simply : creation of a Ray + dictionnary lookup
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("TemplateFolder/TouchColliderManager")]
	public class TouchColliderManager : UniSingleton.Singleton<TouchColliderManager>
	{
		class RaycastCommand
		{
			Ray ray;
			TouchColliderRaycastParameters raycastParameters;

			public RaycastCommand(Ray ray, TouchColliderRaycastParameters raycastParameters)
			{
				this.ray = ray;
				this.raycastParameters = raycastParameters;
			}

			// Two command are equals if they ray and raycast parameters are equals
			// Work because natively two rays created with the same camera and mouse position
			// are equals
			public override bool Equals(object otherObject) =>
			otherObject is RaycastCommand other &&
				(other.ray, other.raycastParameters)
				.Equals((ray, raycastParameters));

			public override int GetHashCode() => (ray, raycastParameters).GetHashCode();

			public RaycastHit[] Execute()
			{
				if(raycastParameters.firstHitOnly)
				{
					RaycastHit hit;

					if(Physics.Raycast(ray, out hit, raycastParameters.MaxDistance, raycastParameters.layerMask))
					{
						return new RaycastHit[]{hit};
					}
					else
					{
						return new RaycastHit[]{};
					}
				}
				else
				{
					return Physics.RaycastAll(ray, raycastParameters.MaxDistance, raycastParameters.layerMask);
				}
			}
		}

		Dictionary<RaycastCommand, RaycastHit[]> raycastCommandHits = new Dictionary<RaycastCommand, RaycastHit[]>();

		public RaycastHit[] Raycast(Ray ray, TouchColliderRaycastParameters raycastParameters)
		{
			RaycastHit[] hits;

			RaycastCommand command = new RaycastCommand(ray, raycastParameters);

			// We save raycast 
			if(raycastCommandHits.TryGetValue(command, out hits))
			{
				//Debug.Log("Reused command : command dictionnary entries count = " + raycastCommandHits.Count);
				return hits;
			}

			hits = command.Execute();

			raycastCommandHits.Add(command, hits);

			return hits;
		}

		void Update()
		{
			raycastCommandHits.Clear();
		}
	}
}