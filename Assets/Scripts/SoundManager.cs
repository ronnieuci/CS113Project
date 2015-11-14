using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
	public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
	public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
	public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched
	public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
	
	
	void Awake ()
	{
		//Check if there is already an instance of SoundManager
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);
	}

	void Start()
	{	}
	
	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.

		// Original
//		efxSource.clip = clip;
//		efxSource.Play ();

		// New 
//		efxSource.PlayClipAtPoint (clip, transform.position);
	}
}
