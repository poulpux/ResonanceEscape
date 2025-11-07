using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static EditorManager;

public class GameManager : MonoSingleton<GameManager>
{
    #region ValueGeneral
    public ELangues _langueActuelle = ELangues.ENGLISH;
    public EGameState _state = EGameState.MENUPLAYMODE;
    #endregion

    #region Events
    [HideInInspector] public UnityEvent _waitingToActEvent = new UnityEvent();
    [HideInInspector] public UnityEvent _overwatchEvent = new UnityEvent();
    [HideInInspector] public UnityEvent _playerActEvent = new UnityEvent();
    [HideInInspector] public UnityEvent _playPlayModeEvent = new UnityEvent();
    [HideInInspector] public UnityEvent _winTheLevelEvent = new UnityEvent();
    [HideInInspector] public UnityEvent _enterInEditModePastEvent = new UnityEvent();
    [HideInInspector] public UnityEvent _enterInEditModeEvent = new UnityEvent();
    #endregion

    #region Callbacks
    private void Start()
    {

        //StartCoroutine(WaitASecond());
        _playerActEvent.AddListener(() =>/*F_WaitingAction()*/StartCoroutine(PlayerMoveCoroutine()));
        _playPlayModeEvent.AddListener(() => PlayPlayMode());
        _winTheLevelEvent.AddListener(() => GoBackToMenu());
        RaycastManager_.I.allTag[GV.TagSO._editorBackToMenu]._click2DEvent.AddListener(() => GoBackToMenu());
        RaycastManager_.I.allTag[GV.TagSO._menuEditorMode]._click2DEvent.AddListener(() => { _state = EGameState.MENUEDITORMODE; _enterInEditModeEvent.Invoke(); });
        RaycastManager_.I.allTag[GV.TagSO._menuPastCode]._click2DEvent.AddListener(() =>  PastBoutonMenu());
        RaycastManager_.I.allTag[GV.TagSO._menuPlayMode]._click2DEvent.AddListener(() => _state = EGameState.MENUPLAYMODE);
        RaycastManager_.I.allTag[GV.TagSO._menuSupport]._click2DEvent.AddListener(() => Application.OpenURL("https://ko-fi.com/ambroise_marquet"));
        RaycastManager_.I.allTag[GV.TagSO._menuInsta]._click2DEvent.AddListener(() => Application.OpenURL("https://www.instagram.com/ambroise.mt/"));
        RaycastManager_.I.allTag[GV.TagSO._menuFiverr]._click2DEvent.AddListener(() => Application.OpenURL("https://fr.fiverr.com/s/zWVveqo"));
        RaycastManager_.I.allTag[GV.TagSO._menuCredit]._click2DEvent.AddListener(() => SceneManager.LoadScene(1)) ;
        //_state = EGameState.EDITOR;
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

    private void PastBoutonMenu()
    {
        MapData newMapData = null;
        newMapData = ReadMap(GUIUtility.systemCopyBuffer);
        if (newMapData == null)
            return;

        print("passe ici");
        EditorManager.I.F_ChangeMap(GUIUtility.systemCopyBuffer);
        _state = EGameState.MENUEDITORMODE;
        _enterInEditModePastEvent.Invoke();
    }

    private IEnumerator WaitASecond()
    {
        yield return new WaitForEndOfFrame();
        F_WaitingAction();
    }

    private void PlayPlayMode()
    {
        _state = EGameState.OVERWATCH;
        _overwatchEvent.Invoke();
    }

    private void GoBackToMenu()
    {
        _state = EGameState.MENUPLAYMODE;
    }

    #endregion
}
