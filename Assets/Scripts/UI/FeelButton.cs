using MoreMountains.Feedbacks;
using Shapes;
using Sirenix.OdinInspector;
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

    [ColorPalette("Resonnance")]
    [SerializeField] Color cubeUnselectColor, cubeSelectColor, outlineCubeUnselectColor, outlineCubeSelectColor, lineUnselectColor, lineSelectColor;

    AnimatingCurve curveCube, curseOutlineCube, curveLine;

    void Start()
    {
        //La base
        RaycastManager_.I.allTag[gameObject.tag]._survole2DEvent.AddListener(() => Survole());
        RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(()=> Select());



        //if (gameObject.tag == GV.TagSO._menuPlayMode)
        //{
        //    Select();
        //    //RaycastManager_.I.allTag[GV.TagSO.edi]._click2DEvent
        //}
    }

    void FixedUpdate()
    {
        //PlayCurve();
        VerifSurvoleback();
    }

    private void PlayCurve()
    {
        float alphaCurveLine = 0f;
        Vector3 colorCurveCube = Vector3.zero;
        Vector3 colorCurveOutlineCube = Vector3.zero;

        Tools.PlayCurve(ref curveLine, ref alphaCurveLine);
        line1.Color = new Color(line1.Color.r , line1.Color.g, line1.Color.b ,alphaCurveLine);
        line2.Color = new Color(line1.Color.r , line1.Color.g, line1.Color.b ,alphaCurveLine);

        Tools.PlayCurve(ref curveCube, ref colorCurveCube);
        carré1.Color = new Color(colorCurveCube.x, colorCurveCube.y, colorCurveCube.z, 1f);
        carré2.Color = new Color(colorCurveCube.x, colorCurveCube.y, colorCurveCube.z, 1f);
        
        Tools.PlayCurve(ref curseOutlineCube, ref colorCurveOutlineCube);
        outline1.Color = new Color(colorCurveOutlineCube.x, colorCurveOutlineCube.y, colorCurveOutlineCube.z, 1f);
        outline2.Color = new Color(colorCurveOutlineCube.x, colorCurveOutlineCube.y, colorCurveOutlineCube.z, 1f);
    }

    private void VerifSurvoleback()
    {
        if (!isSelected && !survole && !feedbackSurvole.IsPlaying && !feedbackSurvoleBack.IsPlaying && Mathf.Abs(carré1.transform.eulerAngles.z - 45f) < 0.1f)
        {
            feedbackSurvoleBack.PlayFeedbacks();
        }
        survole = false;
    }

    private void Survole()
    {
        survole = true;
        if (!feedbackSurvole.IsPlaying && !feedbackSurvoleBack.IsPlaying && carré1.transform.eulerAngles.z == 0f && !isSelected)
            feedbackSurvole.PlayFeedbacks();
    }

    private void Select()
    {
        if (!isSelected && !feedbackSelect.IsPlaying && !feedbackSelectBack.IsPlaying)
        {
            curveLine = new AnimatingCurve(0, 1, 0.3f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            curveCube = new AnimatingCurve(new Vector3(cubeUnselectColor.r, cubeUnselectColor.g, cubeUnselectColor.b), new Vector3(cubeSelectColor.r, cubeSelectColor.g, cubeSelectColor.b), 0.3f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            curseOutlineCube = new AnimatingCurve(new Vector3(outlineCubeUnselectColor.r, outlineCubeUnselectColor.g, outlineCubeUnselectColor.b), new Vector3(outlineCubeSelectColor.r, outlineCubeSelectColor.g, outlineCubeSelectColor.b), 0.3f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            feedbackSelect.PlayFeedbacks();
            isSelected = true;
            Survole();
        }
    }

    private void Unselect()
    {
        if (isSelected && !feedbackSelect.IsPlaying && !feedbackSelectBack.IsPlaying)
        {
            curveLine = new AnimatingCurve(1f, 0f, 0.3f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            curveCube = new AnimatingCurve(new Vector3(cubeSelectColor.r, cubeSelectColor.g, cubeSelectColor.b), new Vector3(cubeUnselectColor.r, cubeUnselectColor.g, cubeUnselectColor.b), 0.3f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            curseOutlineCube = new AnimatingCurve(new Vector3(outlineCubeSelectColor.r, outlineCubeSelectColor.g, outlineCubeSelectColor.b), new Vector3(outlineCubeUnselectColor.r, outlineCubeUnselectColor.g, outlineCubeUnselectColor.b), 0.3f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            feedbackSelectBack.PlayFeedbacks();
            isSelected = false;
            VerifSurvoleback();
        }
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
