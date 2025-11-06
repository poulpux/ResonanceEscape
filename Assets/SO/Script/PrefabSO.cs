using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabSO", menuName = "SO/Prefab")]
public class PrefabSO : ScriptableObject
{
    [Header("Map")]
    public GameObject _largeMap;
    public GameObject _longMap, _bothMap;
    [Header("Grille")]
    public GameObject _largeMapGrille;
    public GameObject _longMapGrille, _bothMapGrille;
    [Header("Editor")]
    public GameObject _player;
    public GameObject _winCondition, _wall, _semiWall, _murBloobPlein, _murBloobVide, _bloobPlein, _bloobVide, _piks, _projectile, _inertieBoost, _star, _blackHole;
}
