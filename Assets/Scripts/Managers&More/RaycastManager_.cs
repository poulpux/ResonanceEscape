using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class RaycastManager_ : MonoSingleton<RaycastManager_>
{
    #region Values
    [Header("Cam")]
    [SerializeField]
    private Camera _camera;

    [Header("UI")]
    [SerializeField]
    private GraphicRaycaster UIRaycaster;

    [SerializeField]
    EventSystem eventSystem;
    PointerEventData pointerEventData;

    public Dictionary<string, RaycastTag> allTag = new Dictionary<string, RaycastTag>();

    #endregion
    #region Callbacks
    protected override void Awake()
    {
        base.Awake();
        InstantiateAllTag();
    }

    void Start()
    {
        if (eventSystem != null)
            pointerEventData = new PointerEventData(eventSystem);
        InputSystem_.I._leftClick._event.AddListener(() => Click());
    }

    private void Update()
    {
        Survole();
    }

    #endregion
    #region Functions
    private void InstantiateAllTag()
    {
        InstantiateOneTag(GV.TagSO._quitParamter);
        InstantiateOneTag(GV.TagSO._parameter);
        InstantiateOneTag(GV.TagSO._playAgain);
        InstantiateOneTag(GV.TagSO._backToMenu);
        InstantiateOneTag(GV.TagSO._crédit);
        InstantiateOneTag(GV.TagSO._giveATips);
        InstantiateOneTag(GV.TagSO._mode);
        InstantiateOneTag(GV.TagSO._play);
        InstantiateOneTag(GV.TagSO._reviewBoard);

        InstantiateOneTag(GV.TagSO._editorPlayer);
        InstantiateOneTag(GV.TagSO._editorWinCondition);
        InstantiateOneTag(GV.TagSO._editorWall);
        InstantiateOneTag(GV.TagSO._editorSemiWall);
        InstantiateOneTag(GV.TagSO._editorMapType);
        InstantiateOneTag(GV.TagSO._editorPlay);
        InstantiateOneTag(GV.TagSO._editorErase);
        InstantiateOneTag(GV.TagSO._editorSave);
        InstantiateOneTag(GV.TagSO._editorClean);
        InstantiateOneTag(GV.TagSO._editorBloobWall);
        InstantiateOneTag(GV.TagSO._editorBloob);
        InstantiateOneTag(GV.TagSO._editorSpike);
        InstantiateOneTag(GV.TagSO._editorProjectile);
        InstantiateOneTag(GV.TagSO._editorInertieBoost);
        InstantiateOneTag(GV.TagSO._editorStar);
        InstantiateOneTag(GV.TagSO._editorBlackHole);

        InstantiateOneTag(GV.TagSO._menuPlayMode);
        InstantiateOneTag(GV.TagSO._menuEditorMode);
        InstantiateOneTag(GV.TagSO._menuPastCode);
        InstantiateOneTag(GV.TagSO._menuHelp);
        InstantiateOneTag(GV.TagSO._menuResetMap);
        InstantiateOneTag(GV.TagSO._menuCredit);
        InstantiateOneTag(GV.TagSO._menuSupport);
        InstantiateOneTag(GV.TagSO._menuPlayModeLeftLevel);
        InstantiateOneTag(GV.TagSO._menuPlayModeRightLevel);
        InstantiateOneTag(GV.TagSO._menuParameter);
        InstantiateOneTag(GV.TagSO._menuPlay);
    }
    private void Click() =>
        HandleRaycast(UI, TwoD, ThreeD, true);

    private void Survole()=>
        HandleRaycast(UI, TwoD, ThreeD, false);
    #endregion
}
