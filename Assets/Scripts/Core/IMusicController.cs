using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using LOT.Core;

namespace LOT.Core
{
    public interface IMusicController
    {
        IEnumerator Initialize (GeneralOptions options);

        void Play (string sound, bool loop = false, float delay = 0);

        IEnumerator PlayAsync (string sound, bool loop = false, float delay = 0);

        void Stop ();

        IEnumerator StopAsync ();

        float GetVolume ();

        void SetVolume (float volume);

        void SaveSoundSettings ();

        void MuteMusic (bool flag);

        bool IsMuteMusic ();
    }
}

