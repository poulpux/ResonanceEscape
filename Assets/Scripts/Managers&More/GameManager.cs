using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    #region ValueGeneral
    public ELangues _langueActuelle = ELangues.ENGLISH;
    public EGameState _state = EGameState.MENU;
    #endregion

    #region Events
    #endregion

    #region Callbacks
    private void Start()
    {
        
    }
    #endregion

    #region Functions
    #endregion
}
