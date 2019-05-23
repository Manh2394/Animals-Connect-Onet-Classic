using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using LOT.Core;

namespace LOT.Core
{
    public interface ISoundController
    {
        IEnumerable Initialize (GeneralOptions options);

        void Play (string sound);

        void PlayDelayed (string sound, float delay);

        IEnumerator PlayAsync (string sound, float delay = 0);

        void Stop ();

        IEnumerator StopAsync ();

        void SaveSoundSettings ();

        void MuteSound (bool flag);

        bool IsMuteSound ();
    }
}
