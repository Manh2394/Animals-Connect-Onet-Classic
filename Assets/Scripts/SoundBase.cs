using UnityEngine;
using System.Collections;
[RequireComponent( typeof( AudioSource ) )]
public class SoundBase : MonoBehaviour {
	public static SoundBase Instance;
	public AudioClip click;
	public AudioClip getScore;
 	public AudioClip coins;
    public AudioClip levelComplete;
    public AudioClip lose;
    public AudioClip win;

    ///SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.kreakWheel );

   // Use this for initialization
	void Awake () {
		if (Instance != null)
			return;
    	if (PlayerPrefs.GetInt("Sound", 1) == 0)
        {
        	this.GetComponent<AudioSource>().volume = 0;
		}
        else
       	{
        	this.GetComponent<AudioSource>().volume = 1;
       	}

		DontDestroyOnLoad(gameObject);
		Instance = this;
	}
}
