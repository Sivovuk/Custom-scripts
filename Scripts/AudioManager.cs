using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public AudioSource forestMusic;
    public AudioSource gameMusic;
    public AudioSource loadSound;
    public AudioSource questSound;

    private static AudioManager instance;

    public static AudioManager Instance => instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(AudioSource audio) 
    {
        if (audio != null) 
        {
            audio.PlayOneShot(audio.clip);
        }
    }

    public void StopAudio(AudioSource audio)
    {
        if (audio != null)
        {
            audio.Stop();
        }
    }
}
