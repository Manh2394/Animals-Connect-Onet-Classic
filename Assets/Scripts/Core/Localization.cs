using System.Collections;
using SimpleJSON;

namespace LOT.Core
{
    public class Localization
    {
        static Localization _instance;

        static public Localization Instance {
            get {
                if (_instance == null)
                    InitInstance ();

                return _instance;
            }
        }

        static void InitInstance ()
        {
            _instance = new Localization ();
        }

        private static JSONNode content;

        private Localization ()
        {
        }

        public IEnumerator Load ()
        {
            yield return null;
        }

        public static string GetText (string key , object data = null, float threshold = default(float))
        {
            return Localization.Instance.GetLocalizedText (key, data, threshold);
        }

        public string GetLocalizedText (string key , object data , float threshold)
        {
            return "*" + key;
        }
    }
}
