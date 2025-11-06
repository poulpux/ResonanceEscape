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
    [SerializeField] GameObject UIMenu, UIPlayMode, UIEditMode;
    [SerializeField] CinemachineCamera  camer;

    void Start()
    {
        RaycastManager_.I.allTag[GV.TagSO._menuEditorMode]._click2DEvent.AddListener(() =>
        {
            UIMenu.SetActive(false);
            UIPlayMode.SetActive(false);
            UIEditMode.SetActive(true);
            camer.Lens.OrthographicSize = 7f;
            camer.transform.position = Vector3.forward * -10f + Vector3.right * -2.11f;
        });
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeLeftLevel]._click2DEvent.AddListener(() => LeftClickLevel());
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeRightLevel]._click2DEvent.AddListener(() => RightClickLevel());
        RaycastManager_.I.allTag[GV.TagSO._menuPlay]._click2DEvent.AddListener(() => ClickOnPlay());
        GameManager.I._winTheLevelEvent.AddListener(() => ReturnToMenu());
    }

    private void ClickOnPlay()
    {
        UIMenu.SetActive(false);
        UIPlayMode.SetActive(false);
        camer.Lens.OrthographicSize = 6f;
        camer.transform.position = Vector3.forward * -10f;
        GameManager.I._playPlayModeEvent.Invoke();
    }

    private void LeftClickLevel()
    {
        _indexMapPlayMode = _indexMapPlayMode == 0 ? GV.GameSO._allMapList.Count-1 : _indexMapPlayMode -= 1;
        _changeLvEvent.Invoke();
    }
    
    private void RightClickLevel()
    {
        _indexMapPlayMode = _indexMapPlayMode == GV.GameSO._allMapList.Count - 1 ? 0 : _indexMapPlayMode += 1;
        _changeLvEvent.Invoke();
    }

    private void ReturnToMenu()
    {
        UIMenu.SetActive(true);
        UIPlayMode.SetActive(true);
        camer.Lens.OrthographicSize = 7.64f;
        camer.transform.position = Vector3.forward * -10f + Vector3.right * -2.11f;
    }
}
