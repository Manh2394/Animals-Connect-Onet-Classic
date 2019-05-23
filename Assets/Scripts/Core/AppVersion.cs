using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using Assets.Phunk.Core;

namespace LOT.Core
{
    public class AppVersion
    {
        static JSONNode config = null;
        static Dictionary<string, int> VERSION = new Dictionary<string, int>() {
            {"en", 20160406}
        };

        static public int GetVersion()
        {
            if (config == null) {
                config = BaseUtils.LoadJSONResource("Config/config");
            }
            return VERSION[config["lang"]];
        }

    }
}
