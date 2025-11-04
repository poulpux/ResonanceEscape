using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabSO", menuName = "SO/Prefab")]
public class PrefabSO : ScriptableObject
{
    [Header("Map")]
    public GameObject _largeMap, _longMap, _bothMap;
    [Header("Editor")]
    public GameObject _player, winCondition, wall, semiWall;
}
