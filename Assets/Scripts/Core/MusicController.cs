using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using Assets.Phunk.Core;

namespace LOT.Core
{
    public class MusicController : MonoBehaviour, IMusicController
    {
        [Range (0, 1)]
        public float volume = 1f;

        // preference key when saving to local storage
        public bool shouldFadeIn = false;
        public float fadeTime = 0.15f;
        public AudioClip[] audioClips;
        public AudioSource source;
        public GameObject go;
        bool isPlaying = false;
        float currentVolume = 0f;

        const string MuteMusicKey = "muteMusic";

        public IEnumerator Initialize (GeneralOptions options)
        {
            yield return null;
            Log.VerboseFormat ("{0}::Initialize", GetType ().Name);

            go = new GameObject ("MusicAudioSource");
            go.transform.parent = transform;
            source = go.AddComponent<AudioSource> ();
            source.name = "MusicAudioSource";

            LoadSoundSettings ();
        }

        void LoadSoundSettings ()
        {
            // if (soundSettings == null) {
            //     soundSettings = new SoundSettings ();
            // }
            // soundSettings.Load ();
            // source.mute = soundSettings [MuteMusicKey].AsBool;
        }

        public void SaveSoundSettings ()
        {
            // if (soundSettings == null) {
            //     soundSettings = new SoundSettings ();
            // }
            // soundSettings.Save ();
        }

        public void MuteMusic (bool flag)
        {
            // source.mute = flag;
            // soundSettings [MuteMusicKey].AsBool = flag;
        }

        public bool IsMuteMusic ()
        {
            return false;
            // return soundSettings [MuteMusicKey].AsBool;
        }

        public float GetVolume ()
        {
            return this.volume;
        }

        public void SetVolume (float volume)
        {
            volume = Mathf.Clamp (volume, 0, 1);
            this.volume = volume;
            source.mute = volume == 0;

            if (isPlaying) {
                Debug.Log ("Volumn " + volume);
                source.volume = volume;
            }
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

        public void Play (string sound, bool loop = false, float delay = 0)
        {
            Log.Verbose (this.GetType ().Name + "::Play " + sound + " " + loop);
            var clip = this.getAudioClipByName (sound);
            if (!clip) {
                Log.Error ("No audio clip " + sound);
                return;
            }

            source.clip = clip;

            if (shouldFadeIn) {
                source.volume = 0;
                currentVolume = 0;
            } else {
                source.volume = volume;
            }

            source.loop = loop;
            source.PlayDelayed (delay);
            isPlaying = true;
        }

        public IEnumerator PlayAsync (string sound, bool loop = false, float delay = 0)
        {
            if (delay == 0) {
                Play (sound, loop);
            } else {
                yield return new WaitForSeconds (delay);
                Play (sound, loop);
            }

            yield return null;
        }

        public void Stop ()
        {
            source.Stop ();
            isPlaying = false;
        }

        public IEnumerator StopAsync ()
        {
            source.Stop ();
            yield return null;
        }

        void Update ()
        {
            if (!isPlaying) {
                return;
            }

            if (shouldFadeIn) {
                currentVolume = Mathf.Lerp (currentVolume, volume, Time.deltaTime * fadeTime);
                source.volume = currentVolume;

            }

            if (!source.isPlaying) {
                isPlaying = false;
            }
        }
    }
}

