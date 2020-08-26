using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public static SoundManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    //public void PlayClick1Sound()
    //{
    //    AudioManager.Instance.PlayAudio(AudioManager.Instance.click1);
    //}

    //public void PlayClick2Sound() 
    //{
    //    AudioManager.Instance.PlayAudio(AudioManager.Instance.click2);
    //}


}
