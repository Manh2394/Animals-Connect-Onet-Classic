using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour {
    public Image soundIcon;
    public Sprite toggleOffIcon;
    public Sprite toggleOnIcon;

    // Use this for initialization
    void OnEnable()
    {
        soundIcon.sprite = PlayerPrefs.GetInt("Sound", 1) == 0 ? toggleOffIcon : toggleOnIcon;
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Sound", 1) == 0)
        {
            SoundBase.Instance.GetComponent<AudioSource>().volume = 1;
            soundIcon.sprite = toggleOnIcon;
        }
        else
        {
            SoundBase.Instance.GetComponent<AudioSource>().volume = 0;
            soundIcon.sprite = toggleOffIcon;
        }
        PlayerPrefs.SetInt("Sound", (int)SoundBase.Instance.GetComponent<AudioSource>().volume);
        PlayerPrefs.Save();
    }
}
