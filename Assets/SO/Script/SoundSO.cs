using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioCustom
{
    public AudioClip _clip;
    public float _volume = 1f;
}

[CreateAssetMenu(fileName = "SoundSO", menuName = "SO/Sound")]
public class SoundSO : ScriptableObject
{
    //All Audio custom values
    [Header("Music")]
    public AudioCustom _menuMusic;
    public AudioCustom _introGameMusic, _loopGameMusic, _loopCreditMusic;

    [Header("UI")]
    public AudioCustom _boutonSurvole;
    public AudioCustom _clicSurvole, _exitUI, _windowSpawn, _errorPast;
    
    [Header("Game")]
    public AudioCustom _moove;
    public AudioCustom _inertia, _death, _pulse, _win, _dropTileEditor, _eraseTileEditor, _reset, _collsion;
}
