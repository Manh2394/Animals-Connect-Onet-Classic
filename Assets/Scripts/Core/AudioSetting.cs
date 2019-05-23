using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour {
	public Image soundIcon;
	public Image musicIcon;
    public Sprite toggleOffIcon;
    public Sprite toggleOnIcon;

    // Use this for initialization
    void OnEnable () {
        musicIcon.sprite = PlayerPrefs.GetInt("Music", 1) == 0 ?  toggleOffIcon : toggleOnIcon;
        soundIcon.sprite = PlayerPrefs.GetInt("Sound", 1) == 0 ? toggleOffIcon : toggleOnIcon;
    }

    public void ToggleSound ()
    {
        if(PlayerPrefs.GetInt("Sound", 1) == 0)
        {
            SoundBase.Instance.GetComponent<AudioSource>().volume = 1;
            soundIcon.sprite = toggleOnIcon;
        }
        else
        {
            SoundBase.Instance.GetComponent<AudioSource>().volume = 0;
            soundIcon.sprite = toggleOffIcon;
        }
        PlayerPrefs.SetInt( "Sound", (int)SoundBase.Instance.GetComponent<AudioSource>().volume );
        PlayerPrefs.Save();
    }
    public void ToggleMusic ()
    {
        if(PlayerPrefs.GetInt("Music", 1) == 0)
        {
            MusicBase.Instance.GetComponent<AudioSource>().volume = 1;
            musicIcon.sprite = toggleOnIcon;
        }
        else
        {
            MusicBase.Instance.GetComponent<AudioSource>().volume = 0;
            musicIcon.sprite = toggleOffIcon;

        }
        PlayerPrefs.SetInt( "Music", (int) MusicBase.Instance.GetComponent<AudioSource>().volume );
        PlayerPrefs.Save();

    }

}
