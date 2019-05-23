using System.Collections;
using Assets.Phunk.Core;
using SimpleJSON;
using UnityEngine;

namespace LOT.Core
{
    public class SoundController : MonoBehaviour, ISoundController
    {
        [Range (0, 1)]
        public float volume = 1f;

        public AudioClip [] audioClips;
        public AudioSource source;
        public GameObject go;

        // SoundSettings soundSettings;
        const string MuteSoundKey = "muteSound";

        public IEnumerable Initialize (GeneralOptions options)
        {
            yield return null;
            Log.VerboseFormat ("{0}::Initialize", GetType ().Name);

            go = new GameObject ("SoundAudioSource");
            go.transform.parent = transform;
            source = go.AddComponent<AudioSource> ();
            source.volume = volume;
            source.name = "SoundAudioSource";

            LoadSoundSettings ();
        }

        void LoadSoundSettings ()
        {
            // if (soundSettings == null) {
            //     soundSettings = new SoundSettings ();
            // }
            // soundSettings.Load ();
            // source.mute = soundSettings [MuteSoundKey].AsBool;
        }

        public void SaveSoundSettings ()
        {
            // if (soundSettings == null) {
            //     soundSettings = new SoundSettings ();
            // }
            // soundSettings.Save ();
        }

        public void MuteSound (bool flag)
        {
            // source.mute = flag;
            // soundSettings [MuteSoundKey].AsBool = flag;
        }

        public bool IsMuteSound ()
        {
            return false;
            // return soundSettings [MuteSoundKey].AsBool;
        }

        AudioClip getAudioClipByName (string name)
        {
            foreach (var audioClip in audioClips) {
                if (audioClip.name == name) {
                    return audioClip;
                }
            }
            return null;
        }

        public void Play (string sound)
        {
            Log.Verbose (this.GetType ().Name + "::Play " + sound);
            var clip = this.getAudioClipByName (sound);
            if (!clip) {
                Log.Error ("No audio clip " + sound);
                return;
            }

            source.PlayOneShot (clip);
        }

        public void PlayDelayed (string sound , float delay)
        {
            StartCoroutine (PlayAsync (sound, delay));
        }

        public IEnumerator PlayAsync (string sound , float delay = 0)
        {
            if (delay == 0) {
                Play (sound);
            } else {
                yield return new WaitForSeconds (delay);
                Play (sound);
            }

            yield return null;
        }

        public void Stop ()
        {
            source.Stop ();
        }

        public IEnumerator StopAsync ()
        {
            source.Stop ();
            yield return null;
        }
    }
}
