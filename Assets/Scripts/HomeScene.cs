using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.Events;
using LOT.Core;
using UnityEngine.UI;

public class HomeScene : BaseScene {

    public DifficultLevel difficultLevel = DifficultLevel.Normal;
    public GameMode gameMode = GameMode.Leisure;

    public GameObject musicBasePrefab;
    public GameObject soundBasePrefab;

    void Start ()
    {
        if (MusicBase.Instance == null) {
            Instantiate (musicBasePrefab);
        }
        if (SoundBase.Instance == null) {
            Instantiate (soundBasePrefab);
        }

        Media.Initialize();
        Media.Show(Media.Type.Banner, null, null);
    }

    public override IEnumerator Initialize(GeneralOptions options)
    {
        yield return null;
    }


    public void OnCloseDialogClick (GameObject dialog)
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        StartCoroutine (CloseDialog (dialog, null));
    }

    public void OnPlayClick ()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
    }

    public void OnSettingClick (GameObject settingDialog)
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        StartCoroutine (ShowSettingDialog (settingDialog));
    }

    IEnumerator ShowSettingDialog (GameObject settingDialog)
    {
        settingDialog.SetActive (true);
        settingDialog.transform.localScale = Vector3.zero;
        yield return null;
        settingDialog.transform.DOScale (Vector3.one, 0.5f);
        
    }

    public void OnClickHardDifficulLevel ()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        difficultLevel = DifficultLevel.Hard;
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        LoadGame (gameMode, difficultLevel);
    }

    public void OnClickNormalDifficulLevel ()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        difficultLevel = DifficultLevel.Normal;
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        LoadGame (gameMode, difficultLevel);
    }

    public void OnDynamic()
    {
        Settings.SaveMode = 0;

        if (Settings.HasSave == 0)
        {
            this.gameMode = GameMode.Time;
            OnClickNormalDifficulLevel();
            Settings.HasSave = 1;
            return;
        }

        LoadGame(GameMode.Time, DifficultLevel.Normal, true);
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        Analytics.LogEvent("Dynamic Mode");
        Media.Destroy(Media.Type.Banner);
    }

    public void OnClassic()
    {
        Settings.SaveMode = 1;

        if (Settings.HasSave == 0)
        {
            gameMode = GameMode.Time;
            OnClickHardDifficulLevel();
            Settings.HasSave = 1;
            return;
        }

        LoadGame(GameMode.Time, DifficultLevel.Hard, true);
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        Analytics.LogEvent("Classic Mode");
        Media.Destroy(Media.Type.Banner);
    }

    public void LoadGame (GameMode gameMode, DifficultLevel difficultLevel, bool isSavedGame = false)
    {
        switch (gameMode)
        {
            case GameMode.Leisure:
                TaskRunner.Instance.Run (SceneManager.Instance.LoadSceneAsync ("LeisureModeScene", GeneralOptions.Create ("difficultLevel", difficultLevel, "gameMode", GameMode.Leisure, "isSavedGame", isSavedGame)));
                return;
            case GameMode.Time:
                TaskRunner.Instance.Run (SceneManager.Instance.LoadSceneAsync ("TimeModeScene", GeneralOptions.Create ("difficultLevel", difficultLevel, "gameMode", GameMode.Time, "isSavedGame", isSavedGame)));
                return;
            case GameMode.Challenge:
                TaskRunner.Instance.Run (SceneManager.Instance.LoadSceneAsync ("ChallengeModeScene", GeneralOptions.Create ("difficultLevel", difficultLevel, "gameMode", GameMode.Challenge, "isSavedGame", isSavedGame)));
                return;
            case GameMode.Survival:
                TaskRunner.Instance.Run (SceneManager.Instance.LoadSceneAsync ("SurvivalModeScene", GeneralOptions.Create ("difficultLevel", difficultLevel, "gameMode", GameMode.Survival, "isSavedGame", isSavedGame)));
                return;
        }
    }

    public void LoadSavedGame()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        if (Settings.HasSave == 0)
        {
            return;
        }
        Settings.HasSave = 0;
        GameMode gameMode = (GameMode)Settings.GameMode;
        DifficultLevel difficultLevel = (DifficultLevel)Settings.DifficultLevel;
        LoadGame(gameMode, difficultLevel, true);
        //LoadGame(GameMode.Time,DifficultLevel.Hard);
    }

	public void rate() {
		//Application.OpenURL ("https://play.google.com/store/apps/details?id=com.titmit.youstupid");
		Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
	}
	public void ShareIntent() {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
        //AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
        //AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
        //intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
        //intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");
        //intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "https://play.google.com/store/apps/details?id=onet.connect.animal.pikachu");
        //AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
        //currentActivity.Call ("startActivity", intentObject);

        NativeShare.Share("https://play.google.com/store/apps/details?id=" + Application.identifier);

    }

    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit ();
		}		
    }

    public void loadScenesOnline()
    {
        Application.LoadLevel("Online");
    }
}
