using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using Assets.Phunk.Core;

namespace LOT.Core
{
    public enum SceneManagerState
    {
        Normal,
        InTransition
    }

    public class SceneManager : MonoBehaviour, ISceneManager
    {
        public static SceneManager Instance;
        private GeneralOptions sceneOptions;
        private SceneManagerState state = SceneManagerState.Normal;
        private string mCurScene;
        private string nextScene;
        private AsyncOperation curAsync = null;

        private LoadingBar loadingBar;

        void Awake () {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }


        public IEnumerator Initialize (Dictionary<string, object> options)
        {
            yield return null;
            Log.Verbose (this.GetType ().Name + "::Initialize");
        }

        public IEnumerator LoadSceneAsync (string sceneName, GeneralOptions options = null, LoadSceneMode mode = LoadSceneMode.Single, LoadingBar loadingBar = null)
        {
            if (state == SceneManagerState.InTransition) {
                yield break;
            }
            yield return null;
            Log.MessageFormat ("{0}::LoadSceneAsync {1}", this.GetType ().Name, sceneName);

            sceneOptions = (options == null) ? GeneralOptions.Create() : options;
            sceneOptions ["previousScene"] = Application.loadedLevelName;
            nextScene = sceneName;
            var async = Application.LoadLevelAsync (sceneName);
            this.loadingBar = loadingBar;

            curAsync = async;
            state = SceneManagerState.InTransition;
            while (!async.isDone) {
                yield return new WaitForSeconds (0.5f);
            }
        }

        public void Update ()
        {
            if (state == SceneManagerState.InTransition) {
                InTransition_Update ();
            }
        }

        void InTransition_Update ()
        {
            if (this.loadingBar) {
                loadingBar.FillAmount (curAsync.progress);
            }
            if (curAsync.isDone) {
                Log.MessageFormat ("{0}::InTransition_Update {1}", this.GetType ().Name, nextScene);

                state = SceneManagerState.Normal;
                Debug.Log ("nextScene: " + nextScene);
                TaskRunner.Instance.Run (GameObject.Find (nextScene).GetComponent<IScene> ().Initialize (sceneOptions));
                mCurScene = nextScene;
                nextScene = null;
                sceneOptions = null;
            }
        }
    }

}
