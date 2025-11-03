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
}
