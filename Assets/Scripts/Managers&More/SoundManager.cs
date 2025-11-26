using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        GameManager.I._playPlayModeEvent.AddListener(() => StartCoroutine(TransitionCoroutine()));
        GameManager.I._goToMenuEvent.AddListener(() => { F_PlayMusic(GV.SoundSO._menuMusic, true); });
        RaycastManager_.I.allTag[GV.TagSO._menuSon]._click2DGameObjectEvent.AddListener((objet) => SetSoundVolume(objet));
        RaycastManager_.I.allTag[GV.TagSO._menuMusic]._click2DGameObjectEvent.AddListener((objet) => SetMusicVolume(objet));

        print(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name == "SampleScene")
            F_PlayMusic(GV.SoundSO._menuMusic, true);
        else
            F_PlayMusic(GV.SoundSO._loopCreditMusic, true);

        soundFXVolume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
    }

    private void SetSoundVolume(GameObject objet)
    {
        soundFXVolume = int.Parse(objet.name) * 0.25f;
        PlayerPrefs.SetFloat("soundVolume", soundFXVolume);
        PlayerPrefs.Save();
    }

    private void SetMusicVolume(GameObject objet)
    {
        musicVolume = int.Parse(objet.name) * 0.25f;
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        float volumClip = music.clip == GV.SoundSO._menuMusic._clip ? GV.SoundSO._menuMusic._volume : GV.SoundSO._loopGameMusic._volume;
        music.volume = musicVolume * volumClip;
        PlayerPrefs.Save();
    }

    #endregion
    #region Functions
    public void F_PlaySound(AudioCustom audio)
    {
        sound.volume = audio._volume * soundFXVolume;
        sound.PlayOneShot(audio._clip);
    }

    public void F_PlayMusic(AudioCustom audio, bool loop)
    {
        if (music.clip == audio._clip)
            return;
        music.clip = audio._clip;
        music.volume = audio._volume * musicVolume;
        music.loop = loop;
        music.Play();
    }
    #endregion

    private IEnumerator TransitionCoroutine()
    {
        F_PlayMusic(GV.SoundSO._introGameMusic, false);
        yield return new WaitForSeconds(GV.SoundSO._introGameMusic._clip.length);

        if (music.clip == GV.SoundSO._introGameMusic._clip)
            F_PlayMusic(GV.SoundSO._loopGameMusic, true);
        else 
            yield return new WaitForEndOfFrame();

    }
}

