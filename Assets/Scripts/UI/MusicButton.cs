using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    public Image musicIcon;
    public Sprite toggleOffIcon;
    public Sprite toggleOnIcon;

    // Use this for initialization
    void OnEnable()
    {
        musicIcon.sprite = PlayerPrefs.GetInt("Music", 1) == 0 ? toggleOffIcon : toggleOnIcon;
    }

    public void ToggleMusic()
    {
        if (PlayerPrefs.GetInt("Music", 1) == 0)
        {
            MusicBase.Instance.GetComponent<AudioSource>().volume = 1;
            musicIcon.sprite = toggleOnIcon;
        }
        else
        {
            MusicBase.Instance.GetComponent<AudioSource>().volume = 0;
            musicIcon.sprite = toggleOffIcon;

        }
        PlayerPrefs.SetInt("Music", (int)MusicBase.Instance.GetComponent<AudioSource>().volume);
        PlayerPrefs.Save();

    }
}