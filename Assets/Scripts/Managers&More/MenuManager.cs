using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using static EditorManager;

public class MenuManager : MonoSingleton<MenuManager>
{
    public int _indexMapPlayMode = 0;
    public int _indexTuto = 0;
    [HideInInspector] public UnityEvent _changeLvEvent = new UnityEvent();
    [HideInInspector] public UnityEvent _endTutoEvent = new UnityEvent();
    [SerializeField] GameObject UIMenu, UIPlayMode, UIEditMode, UIInGame, UIReplay, lockedBackground, shapes, parameter, UIHelp;
    public List<float> _heightScoreList = new List<float>();
    [SerializeField] CinemachineCamera  camer;
    float timerCollision = 0f;
    [SerializeField] List<GameObject> AllTutoWindow = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < 10; i++)
            _heightScoreList.Add(99.99f);
    }

    void Start()
    {
        RaycastManager_.I.allTag[GV.TagSO._menuEditorMode]._click2DEvent.AddListener(() =>EnterEditor());
        RaycastManager_.I.allTag[GV.TagSO._menuPastCode]._click2DEvent.AddListener(() =>Past());
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeLeftLevel]._click2DEvent.AddListener(() => LeftClickLevel());
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeRightLevel]._click2DEvent.AddListener(() => RightClickLevel());
        RaycastManager_.I.allTag[GV.TagSO._menuPlay]._click2DEvent.AddListener(() => ClickOnPlay());
        RaycastManager_.I.allTag[GV.TagSO._editorPlay]._click2DEvent.AddListener(() => ClickOnPlay());
        RaycastManager_.I.allTag[GV.TagSO._editorBackToMenu]._click2DEvent.AddListener(() => ReturnToMenu());
        RaycastManager_.I.allTag[GV.TagSO._menuHelp]._click2DEvent.AddListener(() => UIHelp.SetActive(true));
        //Ajouter le oui ou non du tuto
        RaycastManager_.I.allTag[GV.TagSO._tutoNon]._click2DEvent.AddListener(() => { _indexTuto = 5; _endTutoEvent.Invoke(); });
        RaycastManager_.I.allTag[GV.TagSO._tutoOui]._click2DEvent.AddListener(() => { StartCoroutine(OuiTutoCoroutine()); });
        //GameManager.I._winTheLevelEvent.AddListener(() => ReturnToMenu());
        GameManager.I._winTheLevelEvent.AddListener(() => /*GoBackToMenu()*/ EnterInReplayMod());
        GameManager.I._goToMenuEvent.AddListener(() => ReturnToMenu());

        InputSystem_.I._leftArrow._event.AddListener(() => { if (GameManager.I._state == EGameState.MENUPLAYMODE) LeftClickLevel(); });
        InputSystem_.I._rightArrow._event.AddListener(() => { if (GameManager.I._state == EGameState.MENUPLAYMODE) RightClickLevel(); });
        InputSystem_.I._leftClick._event.AddListener(()=> { if (GameManager.I._state == EGameState.OVERWATCH) LeftClickOnTuto(); });
        InputSystem_.I._rightClick._event.AddListener(()=> { if (GameManager.I._state == EGameState.OVERWATCH) RightClickOnTuto(); });

        UIMenu.SetActive(true);
        UIPlayMode.SetActive(true);
        UIInGame.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (GameManager.I._state == EGameState.MENUPLAYMODE)
            timerCollision += Time.deltaTime;
    }

    private void EnterInReplayMod()
    {
        UIReplay.SetActive(true );
        UIInGame.SetActive(false );
    }

    private void ClickOnPlay()
    {
        if (timerCollision < 0.2f)
            return;
        //if (!(PlayerPrefs.GetFloat((_indexMapPlayMode - 1).ToString(), 99.99f) != 99.99f || _indexMapPlayMode == 0))
        //    return;
        TryTuto();
        UIMenu.SetActive(false);
        UIPlayMode.SetActive(false);
        UIReplay.SetActive(false);
        UIInGame.SetActive(true) ;
        camer.Lens.OrthographicSize = 6f;
        camer.transform.position = Vector3.forward * -10f;
        timerCollision = 0f;
        GameManager.I._playPlayModeEvent.Invoke();
    }

    private void TryTuto()
    {
        if(_indexMapPlayMode == 0)
            AppearTuto();
    }

    private void AppearTuto()
    {
        foreach (var item in AllTutoWindow)
            item.SetActive(false);

        if(_indexTuto <= 4 && _indexTuto >= 0)
            AllTutoWindow[_indexTuto].SetActive(true);
    }

    private void LeftClickOnTuto()
    {
        if(_indexTuto == 4)
            _endTutoEvent.Invoke();
        if (_indexTuto <= 4 && _indexTuto >= 1)
            _indexTuto++;
        TryTuto();

    }
    
    private void RightClickOnTuto()
    {
        if (_indexTuto <= 4 && _indexTuto >= 2)
            _indexTuto--;
        TryTuto();
    }

    private IEnumerator OuiTutoCoroutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _indexTuto++; TryTuto();
    }

    private void Past()
    {
        MapData newMapData = null;
        newMapData = ReadMap(GUIUtility.systemCopyBuffer);
        if (newMapData == null)
            return;
        
        EnterEditor();
    }

    private void EnterEditor()
    {
        UIMenu.SetActive(false);
        UIPlayMode.SetActive(false);
        UIEditMode.SetActive(true);
        parameter.SetActive(false); 
        camer.Lens.OrthographicSize = 12f;
        camer.transform.position = Vector3.forward * -10f + Vector3.right * -2.11f;
    }

    private void LeftClickLevel()
    {
        _indexMapPlayMode = _indexMapPlayMode == 0 ? GV.GameSO._allMapList.Count-1 : _indexMapPlayMode -= 1;

        _changeLvEvent.Invoke();
        //lockedBackground.SetActive(PlayerPrefs.GetFloat((_indexMapPlayMode - 1).ToString(), 99.99f) == 99.99f && _indexMapPlayMode != 0);
        //shapes.SetActive(PlayerPrefs.GetFloat((_indexMapPlayMode-1).ToString(), 99.99f) != 99.99f || _indexMapPlayMode == 0);
    }
    
    private void RightClickLevel()
    {
        _indexMapPlayMode = _indexMapPlayMode == GV.GameSO._allMapList.Count - 1 ? 0 : _indexMapPlayMode += 1;
        _changeLvEvent.Invoke();
        //lockedBackground.SetActive(PlayerPrefs.GetFloat((_indexMapPlayMode - 1).ToString(), 99.99f) == 99.99f && _indexMapPlayMode != 0);
        //shapes.SetActive(PlayerPrefs.GetFloat((_indexMapPlayMode - 1).ToString(), 99.99f) != 99.99f || _indexMapPlayMode == 0);
    }

    private void ReturnToMenu()
    {
        UIMenu.SetActive(true);
        UIPlayMode.SetActive(true);
        UIReplay.SetActive(false);
        UIEditMode.SetActive(false);
        UIInGame.SetActive(false);
        parameter.SetActive(true) ;
        camer.Lens.OrthographicSize = 7.64f;
        camer.transform.position = Vector3.forward * -10f + Vector3.right * -2.11f;
        timerCollision = 0f;
        _indexTuto = 0;
    }
}
