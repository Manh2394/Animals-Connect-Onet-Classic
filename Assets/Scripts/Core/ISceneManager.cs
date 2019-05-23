using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using LOT.Core;

namespace LOT.Core
{
    public interface ISceneManager
    {
        IEnumerator Initialize (Dictionary<string, object> options);

        IEnumerator LoadSceneAsync (string sceneName, GeneralOptions options = null, LoadSceneMode mode = LoadSceneMode.Single, LoadingBar loadingBar = null);

    }
}
