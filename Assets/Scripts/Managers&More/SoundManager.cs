using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    #region Values
    [HideInInspector] public float musicVolume = 1f, soundFXVolume = 1f;
    [SerializeField] AudioSource sound;
    [SerializeField] AudioSource music;
    #endregion
    #region Callbacks
    void Start()
    {

    }

    #endregion
    #region Functions
    public void F_PlaySound(AudioCustom audio)
    {
        sound.volume = audio._volume * soundFXVolume;
        sound.PlayOneShot(audio._clip);
    }

    private void F_PlayMusic(AudioCustom audio, bool loop)
    {
        if (music.clip == audio._clip)
            return;
        music.clip = audio._clip;
        music.volume = audio._volume * musicVolume;
        music.loop = loop;
        music.Play();
    }
    #endregion
}

