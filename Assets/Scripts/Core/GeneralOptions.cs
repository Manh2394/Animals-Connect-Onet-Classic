using System;
using System.Collections.Generic;

namespace LOT.Core
{
    public class GeneralOptions: Dictionary<string, object>
    {
        public static GeneralOptions Create (params object[] items)
        {
            GeneralOptions result = new GeneralOptions ();
            int length = items.Length;
            if ((length % 2) != 0) {
                throw new Exception ("Number of items not even!");
            }
            int halfLength = length / 2;
            for (int i = 0; i < halfLength; i++) {
                result.Add ((string)items [i * 2], items [i * 2 + 1]);
            }
            return result;
        }
    }
}

