using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using static EditorManager;

public class MenuManager : MonoSingleton<MenuManager>
{
    public int _indexMapPlayMode = 0;
    [HideInInspector] public UnityEvent _changeLvEvent = new UnityEvent();
    [SerializeField] GameObject UIMenu, UIPlayMode, UIEditMode, UIInGame, UIReplay, lockedBackground, shapes, parameter, UIHelp;
    [SerializeField] CinemachineCamera  camer;

    void Start()
    {
        RaycastManager_.I.allTag[GV.TagSO._menuEditorMode]._click2DEvent.AddListener(() =>EnterEditor());
        RaycastManager_.I.allTag[GV.TagSO._menuPastCode]._click2DEvent.AddListener(() =>Past());
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeLeftLevel]._click2DEvent.AddListener(() => LeftClickLevel());
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeRightLevel]._click2DEvent.AddListener(() => RightClickLevel());
        RaycastManager_.I.allTag[GV.TagSO._menuPlay]._click2DEvent.AddListener(() => ClickOnPlay());
        RaycastManager_.I.allTag[GV.TagSO._editorBackToMenu]._click2DEvent.AddListener(() => ReturnToMenu());
        RaycastManager_.I.allTag[GV.TagSO._menuHelp]._click2DEvent.AddListener(() => UIHelp.SetActive(true));
        //GameManager.I._winTheLevelEvent.AddListener(() => ReturnToMenu());
        GameManager.I._winTheLevelEvent.AddListener(() => /*GoBackToMenu()*/ EnterInReplayMod());
        GameManager.I._goToMenuEvent.AddListener(() => ReturnToMenu()); 


        UIMenu.SetActive(true);
        UIPlayMode.SetActive(true);
        UIInGame.SetActive(false);
    }

    private void EnterInReplayMod()
    {
        UIReplay.SetActive(true );
        UIInGame.SetActive(false );
    }

    private void ClickOnPlay()
    {
        if (!(PlayerPrefs.GetFloat((_indexMapPlayMode - 1).ToString(), 99.99f) != 99.99f || _indexMapPlayMode == 0))
            return;
        UIMenu.SetActive(false);
        UIPlayMode.SetActive(false);
        UIReplay.SetActive(false);
        UIInGame.SetActive(true) ;
        camer.Lens.OrthographicSize = 6f;
        camer.transform.position = Vector3.forward * -10f;
        GameManager.I._playPlayModeEvent.Invoke();
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
        lockedBackground.SetActive(PlayerPrefs.GetFloat((_indexMapPlayMode - 1).ToString(), 99.99f) == 99.99f && _indexMapPlayMode != 0);
        shapes.SetActive(PlayerPrefs.GetFloat((_indexMapPlayMode-1).ToString(), 99.99f) != 99.99f || _indexMapPlayMode == 0);
    }
    
    private void RightClickLevel()
    {
        _indexMapPlayMode = _indexMapPlayMode == GV.GameSO._allMapList.Count - 1 ? 0 : _indexMapPlayMode += 1;
        _changeLvEvent.Invoke();
        lockedBackground.SetActive(PlayerPrefs.GetFloat((_indexMapPlayMode - 1).ToString(), 99.99f) == 99.99f && _indexMapPlayMode != 0);
        shapes.SetActive(PlayerPrefs.GetFloat((_indexMapPlayMode - 1).ToString(), 99.99f) != 99.99f || _indexMapPlayMode == 0);
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
    }
}
