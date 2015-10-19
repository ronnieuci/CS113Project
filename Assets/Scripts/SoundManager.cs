using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
    public AudioClip crincleAudioClip;
    AudioSource c;


    void Awake()
    {
        c = AddAudio(crincleAudioClip);
    }

    AudioSource AddAudio( AudioClip audioClip)
    {
        AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
        return audioSource;
    }

    public void PlayCrincle()
    {
        c.Play();
    }
}
