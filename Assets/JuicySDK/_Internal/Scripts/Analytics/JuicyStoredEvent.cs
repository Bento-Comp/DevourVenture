using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Juicy;

namespace JuicyInternal
{
    public class JuicyStoredEvent
    {
        public string name { get; private set; }
        public List<EventProperty> properties { get; private set; }

        public JuicyStoredEvent(string name, List<EventProperty> properties)
        {
            this.name = name;
            this.properties = new List<EventProperty>(properties);
        }
    }
}
