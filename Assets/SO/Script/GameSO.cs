using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSO", menuName = "SO/Game")]
public class GameSO : ScriptableObject
{
    [Header("Player")]
    public float _pulseIntervale;
    public float _maxJumpDistance;

    [Header("Levels")]
    [MultiLineProperty]
    public List<string> _allMapList = new List<string>();
}
