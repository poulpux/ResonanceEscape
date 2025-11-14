using MoreMountains.Feedbacks;
using Shapes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeelButton : MonoBehaviour
{
    [SerializeField] EINTERACTIONTYPE interactionType;
    [SerializeField] ETAGTYPE tagType;
    [SerializeField] Rectangle carré1, carré2, outline1, outline2;
    [SerializeField] TextMeshPro text;
    [SerializeField] Line line1, line2;
    [SerializeField] MMF_Player feedbackSurvole, feedbackSurvoleBack, feedbackSelect, feedbackSelectBack;
    bool isSelected;
    bool survole;


    void Start()
    {
        RaycastManager_.I.allTag[gameObject.tag]._survole2DEvent.AddListener(() => Survole());
    }

    void FixedUpdate()
    {
        VerifSurvoleback();
    }

    private void VerifSurvoleback()
    {
        if(!survole && !feedbackSurvole.IsPlaying && !feedbackSurvoleBack.IsPlaying && carré1.transform.eulerAngles.z == 45f)
            feedbackSurvoleBack.PlayFeedbacks();
        survole = false;
    }

    private void Survole()
    {
        survole = true;
        if (!feedbackSurvole.IsPlaying && !feedbackSurvoleBack.IsPlaying && carré1.transform.eulerAngles.z == 0f)
            feedbackSurvole.PlayFeedbacks();
    }

    enum EINTERACTIONTYPE
    {
        SELECTIONNABLE,
        CLIQUABLE
    }

    enum ETAGTYPE
    {
        MENUPLAY,
        MENUPLAYMODE,
        MENUEDITORMODE,
        MENUPASTCODE,
        MENUHELP,
        MENUCREDIT,
        MENUSUPPORT,
        MENULANGAGE,
        MENULANGAGECASES
    }
}
