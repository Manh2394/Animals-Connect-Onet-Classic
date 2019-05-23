using UnityEngine;
using System.Collections;

namespace LOT.Core
{
    public class DontDestroy : MonoBehaviour
    {
        void Awake ()
        {
            DontDestroyOnLoad (this.gameObject);
        }
    }
}

