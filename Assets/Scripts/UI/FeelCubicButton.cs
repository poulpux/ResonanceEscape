using MoreMountains.Feedbacks;
using Shapes;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeelCubicButton : MonoBehaviour
{
    [SerializeField] Rectangle carré1, outline1;
    [SerializeField] MMF_Player feedbackSurvole, feedbackSurvoleBack;
    bool isSelected;
    bool survole, firstCurve;

    [ColorPalette("Resonnance")]
    [SerializeField] Color cubeUnselectColor, cubeSelectColor, outlineCubeUnselectColor, outlineCubeSelectColor;

    AnimatingCurve curveCube, curveOutlineCube;

    void Start()
    {
        //La base
        if (gameObject.tag != GV.TagSO._menuLangageSelectionCase)
        {
            RaycastManager_.I.allTag[gameObject.tag]._survole2DEvent.AddListener(() => Survole());
            //RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => Select());
        }
        else
        {
            GameManager.I._setRightLanguageEvent.AddListener(() => { if ((ELangues)Enum.Parse(typeof(ELangues), gameObject.name) == GameManager.I._langueActuelle) Select(); else if (isSelected) Unselect(); });
            RaycastManager_.I.allTag[gameObject.tag]._survole2DGameObjectEvent.AddListener((objet) => { if (gameObject.name == objet.name) Survole(); });
            //RaycastManager_.I.allTag[gameObject.tag]._click2DGameObjectEvent.AddListener((objet) => { { if (!isSelected && gameObject.name == objet.name) Select(); else if (isSelected && gameObject.name == objet.name) Unselect(); } });
        }

        RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => StartCoroutine(ClickOnButtonCoroutine()));
    }

    void FixedUpdate()
    {
        PlayCurve();
        VerifSurvoleback();
        VerifSelectedPourSurvole();
    }

    private IEnumerator ClickOnButtonCoroutine()
    {
        Select();
        yield return new WaitForSeconds(0.3f);
        Unselect();
    }

    private void VerifSelectedPourSurvole()
    {
        if (isSelected && Mathf.Abs(carré1.transform.eulerAngles.z - 45f) > 0.1f)
            feedbackSurvole.PlayFeedbacks();
    }

    private void PlayCurve()
    {
        if (!firstCurve)
            return;///

        Vector3 colorCurveCube = Vector3.zero;
        Vector3 colorCurveOutlineCube = Vector3.zero;


        Tools.PlayCurve(ref curveCube, ref colorCurveCube);
        carré1.Color = new Color(colorCurveCube.x, colorCurveCube.y, colorCurveCube.z, 1f);

        Tools.PlayCurve(ref curveOutlineCube, ref colorCurveOutlineCube);
        outline1.Color = new Color(colorCurveOutlineCube.x, colorCurveOutlineCube.y, colorCurveOutlineCube.z, 1f);

        if (Tools.isCurveFinish(curveCube))
        {
            carré1.Color = new Color(curveCube.endValue.x, curveCube.endValue.y, curveCube.endValue.z, 1f);

            outline1.Color = new Color(curveOutlineCube.endValue.x, curveOutlineCube.endValue.y, curveOutlineCube.endValue.z, 1f);
        }
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
        if (!isSelected)
        {
            curveCube = new AnimatingCurve(new Vector3(cubeUnselectColor.r, cubeUnselectColor.g, cubeUnselectColor.b), new Vector3(cubeSelectColor.r, cubeSelectColor.g, cubeSelectColor.b), 0.15f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            curveOutlineCube = new AnimatingCurve(new Vector3(outlineCubeUnselectColor.r, outlineCubeUnselectColor.g, outlineCubeUnselectColor.b), new Vector3(outlineCubeSelectColor.r, outlineCubeSelectColor.g, outlineCubeSelectColor.b), 0.15f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            Survole();
            isSelected = true;
            firstCurve = true;
        }
    }

    private void Unselect()
    {
        if (isSelected)
        {
            curveCube = new AnimatingCurve(new Vector3(cubeSelectColor.r, cubeSelectColor.g, cubeSelectColor.b), new Vector3(cubeUnselectColor.r, cubeUnselectColor.g, cubeUnselectColor.b), 0.3f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            curveOutlineCube = new AnimatingCurve(new Vector3(outlineCubeSelectColor.r, outlineCubeSelectColor.g, outlineCubeSelectColor.b), new Vector3(outlineCubeUnselectColor.r, outlineCubeUnselectColor.g, outlineCubeUnselectColor.b), 0.3f, GRAPH.EASECUBIC, INANDOUT.IN, LOOP.CLAMP);
            isSelected = false;
            VerifSurvoleback();
        }
    }

    private void OnEnable()
    {
        if (!isSelected)
        {
            feedbackSurvole.StopFeedbacks();
            feedbackSurvoleBack.StopFeedbacks();
        }
    }
}
