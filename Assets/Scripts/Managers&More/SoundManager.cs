using MoreMountains.Feedbacks;
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
        RaycastManager_.I.allTag[GV.TagSO._menuHelp]._click2DEvent.AddListener(() => F_PlaySound(GV.SoundSO._windowSpawn));
        GameManager.I._pastErrorEvent.AddListener(() => F_PlaySound(GV.SoundSO._errorPast));
        GameManager.I._winTheLevelFeedbackEvent.AddListener(() => F_PlaySound(GV.SoundSO._win));
        InputSystem_.I._r._event.AddListener(() => { if (!PlayerMovement.I._isDead &&(GameManager.I._state == EGameState.WAITINGACTION || GameManager.I._state == EGameState.ACT || GameManager.I._state == EGameState.OVERWATCH)) { F_PlaySound(GV.SoundSO._reset); print("passe"); } });
        GameManager.I._pulseEvent.AddListener(()=> F_PlaySound(GV.SoundSO._pulse));
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

