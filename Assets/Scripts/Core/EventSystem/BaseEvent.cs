using System;
using UnityEngine;
using LOT.Core;

namespace LOT.Events
{
    public class BaseEvent
    {
        public readonly string type;
        public readonly float time;
        public readonly int intParam;
        public readonly float floatParam;
        public readonly string stringParam;
        public readonly object objectParam;
        public readonly GeneralOptions generalOptionsParam;

        public readonly object sender;
        public readonly object origin;

        private BaseEvent (string type, float time = 0, object sender = null, object origin = null, int intParam = 0, float floatParam = 0, string stringParam = "", object objectParam = null, GeneralOptions generalOptionsParam = null)
        {
            this.type = type;
            this.time = time;
            this.sender = sender;
            this.origin = origin;
            this.intParam = intParam;
            this.floatParam = floatParam;
            this.stringParam = stringParam;
            this.objectParam = objectParam;
            this.generalOptionsParam = generalOptionsParam;
        }

        public static BaseEvent Create (string type, float time = 0, object sender = null, object origin = null, int intParam = 0, float floatParam = 0, string stringParam = "", object objectParam = null, GeneralOptions generalOptionsParam = null)
        {
            return new BaseEvent(type, time, sender, origin, intParam, floatParam, stringParam, objectParam, generalOptionsParam);
        }

        public static BaseEvent CreateSimple(string type, object sender = null, int intParam = 0, float floatParam = 0, string stringParam = "", object objectParam = null)
        {
            return BaseEvent.Create(type, 0, sender, sender, intParam, floatParam, stringParam, objectParam);
        }

        public static BaseEvent CreateTimedSimple (string type, object sender = null, int intParam = 0, float floatParam = 0, string stringParam = "", object objectParam = null)
        {
            return BaseEvent.Create(type, Time.time, sender, sender, intParam, floatParam, stringParam, objectParam);
        }

        public static BaseEvent CreateOptions (string type, object sender = null, GeneralOptions generalOptionsParam = null)
        {
            return BaseEvent.Create(type, 0, sender, sender, 0, 0, "", null, generalOptionsParam);
        }

        public static BaseEvent CreateTimedOptions (string type, object sender = null, GeneralOptions generalOptionsParam = null)
        {
            return BaseEvent.Create(type, Time.time, sender, sender, 0, 0, "", null, generalOptionsParam);
        }

        public static BaseEvent CloneWithNewSender (BaseEvent e, object sender) 
        {
            return BaseEvent.Create(e.type, e.time, sender, e.origin, e.intParam, e.floatParam, e.stringParam, e.objectParam, e.generalOptionsParam);
        }
    }
}

