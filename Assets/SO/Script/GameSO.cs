using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSO", menuName = "SO/Game")]
public class GameSO : ScriptableObject
{
    [Header("Player")]
    public float _pulseIntervale;
    public float _maxJumpDistance;
}
