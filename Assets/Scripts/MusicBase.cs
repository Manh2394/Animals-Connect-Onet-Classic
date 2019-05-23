using UnityEngine;
using System.Collections;

public class MusicBase : MonoBehaviour {
	public static MusicBase Instance;

	// Use this for initialization
	void Awake () {
		if (Instance != null)
			return;
    	if (PlayerPrefs.GetInt("Music", 1) == 0)
       	{
         	this.GetComponent<AudioSource>().volume = 0;
        }
       	else
       	{
        	this.GetComponent<AudioSource>().volume = 1;
        }
		DontDestroyOnLoad (this);
		Instance = this;
	}
}
