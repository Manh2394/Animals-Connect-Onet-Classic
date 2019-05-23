using UnityEngine;
using System.Collections;
using LOT.Core;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GameScene : BaseScene {
	public GameManager gameManager;
	public GameObject pause;
	void Awake ()
	{
        //PlayerPrefs.SetInt("money", 0);
		// AdmobRequest.Instance.RequestBanner ().Hide ();
	}
    public override IEnumerator Initialize(GeneralOptions options)
    {
        yield return null;
        gameManager.Initialize (options, this);
    }
	public void OnPauseClick ()
	{
        HUD1.Instance.ShowPausePopup();
        Media.Show(Media.Type.Banner, null, null);
	}
	public void OnResumeClick (GameObject dialog)
	{
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);
		gameManager.isPause = false;
		this.OnDialogClose (dialog);
	}
	public void OnExitClick ()
	{
		gameManager.SaveGame ();
    	TaskRunner.Instance.Run (SceneManager.Instance.LoadSceneAsync ("Home", null));
	}
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			gameManager.isPause = true;
			this.OnShowDialog (pause);
		}		
	}

}
