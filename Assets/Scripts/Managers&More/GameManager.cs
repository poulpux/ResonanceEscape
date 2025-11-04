using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoSingleton<GameManager>
{
    #region ValueGeneral
    public ELangues _langueActuelle = ELangues.ENGLISH;
    public EGameState _state = EGameState.MENU;
    #endregion

    #region Events
    [HideInInspector] public UnityEvent _waitingToActEvent = new UnityEvent();
    [HideInInspector] public UnityEvent _playerActEvent = new UnityEvent();
    #endregion

    #region Callbacks
    private void Start()
    {

        //StartCoroutine(WaitASecond());
        _playerActEvent.AddListener(()=>/*F_WaitingAction()*/StartCoroutine(PlayerMoveCoroutine()));
        _state = EGameState.EDITOR;
    }
    #endregion

    #region Functions

    public void F_WaitingAction()
    {
        _state = EGameState.WAITINGACTION;
        ChangeTimeScale(0f);
        _waitingToActEvent.Invoke();
    }

    private void ChangeTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
    }

    private IEnumerator PlayerMoveCoroutine()
    {
        ChangeTimeScale(1f);
        yield return new WaitForSeconds(GV.GameSO._pulseIntervale);
        F_WaitingAction();
    }

    private IEnumerator WaitASecond()
    {
        yield return new WaitForEndOfFrame();
        F_WaitingAction();
    }

    #endregion
}
