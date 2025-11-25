using MoreMountains.Feedbacks;
using Shapes;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class FeelCubicButton : MonoBehaviour
{
    [SerializeField] bool selectible = false;

    [SerializeField] Rectangle carré1, outline1;
    [SerializeField] MMF_Player feedbackSurvole, feedbackSurvoleBack;
    bool isSelected;
    bool survole, firstCurve;
    [ColorPalette("Resonnance")]
    [SerializeField] Color cubeUnselectColor, cubeSelectColor, outlineCubeUnselectColor, outlineCubeSelectColor;

    AnimatingCurve curveCube, curveOutlineCube;
    //Coroutine coroutine;
    void Start()
    {
        //La base
        if(gameObject.tag == GV.TagSO._menuMusic || gameObject.tag == GV.TagSO._menuSon)
        {
            RaycastManager_.I.allTag[gameObject.tag]._survole2DGameObjectEvent.AddListener((objet) => { if (objet.name == gameObject.name) Survole(); });
        }
        else if (gameObject.tag != GV.TagSO._menuLangageSelectionCase)
        {
            RaycastManager_.I.allTag[gameObject.tag]._survole2DEvent.AddListener(() => Survole());
            //RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => Select());
        }

        if(!selectible)
            RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => /*coroutine =*/ StartCoroutine(ClickOnButtonCoroutine()));
        else if(gameObject.tag == GV.TagSO._editorBlackHole || gameObject.tag == GV.TagSO._editorBloob ||
            gameObject.tag == GV.TagSO._editorBloobWall || gameObject.tag == GV.TagSO._editorSemiWall ||
            gameObject.tag == GV.TagSO._editorSpike || gameObject.tag == GV.TagSO._editorPlayer ||
            gameObject.tag == GV.TagSO._editorWinCondition || gameObject.tag == GV.TagSO._editorWall)
        {
            RaycastManager_.I.allTag[GV.TagSO._editorBlackHole]._click2DEvent.AddListener(() => {  if (gameObject.tag == GV.TagSO._editorBlackHole) Select(); else Unselect(); });
            RaycastManager_.I.allTag[GV.TagSO._editorBloob]._click2DEvent.AddListener(() => {if (gameObject.tag == GV.TagSO._editorBloob) Select(); else Unselect(); });
            RaycastManager_.I.allTag[GV.TagSO._editorBloobWall]._click2DEvent.AddListener(() => { if (gameObject.tag == GV.TagSO._editorBloobWall) Select(); else Unselect(); });
            RaycastManager_.I.allTag[GV.TagSO._editorSemiWall]._click2DEvent.AddListener(() => { if (gameObject.tag == GV.TagSO._editorSemiWall) Select(); else Unselect(); });
            RaycastManager_.I.allTag[GV.TagSO._editorSpike]._click2DEvent.AddListener(() => {  if (gameObject.tag == GV.TagSO._editorSpike) Select(); else Unselect(); });
            RaycastManager_.I.allTag[GV.TagSO._editorPlayer]._click2DEvent.AddListener(() => {  if (gameObject.tag == GV.TagSO._editorPlayer) Select(); else Unselect(); });
            RaycastManager_.I.allTag[GV.TagSO._editorWinCondition]._click2DEvent.AddListener(() => { if (gameObject.tag == GV.TagSO._editorWinCondition) Select(); else Unselect(); });
            RaycastManager_.I.allTag[GV.TagSO._editorWall]._click2DEvent.AddListener(() => { if (gameObject.tag == GV.TagSO._editorWall) Select(); else Unselect(); });

            GameManager.I._enterInEditModeEvent.AddListener(() => { if (gameObject.tag == GV.TagSO._editorPlayer) Select(); else Unselect(); });
            GameManager.I._enterInEditModePastEvent.AddListener(() => { if (gameObject.tag == GV.TagSO._editorPlayer) Select(); else Unselect(); });
        }
        else if (gameObject.tag == GV.TagSO._menuParameter)
        {
            //Je pense que la logique est pas bonne, car quand je déselectionne selectible, tout marche
            RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => { print("passe"); if (isSelected) Unselect(); else Select(); });
        }
        else if (gameObject.tag == GV.TagSO._menuMusic || gameObject.tag == GV.TagSO._menuSon)
        {
            //Pop
            RaycastManager_.I.allTag[gameObject.tag]._click2DGameObjectEvent.AddListener((objet) => { if (int.Parse(objet.name) >= int.Parse(gameObject.name) && 0 != int.Parse(gameObject.name)) Select(); else if(0 != int.Parse(gameObject.name)) Unselect(); if (int.Parse(objet.name) == 0 && 0 == int.Parse(gameObject.name)) Select(); else if(0 == int.Parse(gameObject.name)) Unselect(); });
            if (gameObject.tag == GV.TagSO._menuMusic)
            {
                int indexMusic = (int)(PlayerPrefs.GetFloat("musicVolume", 0.5f) * 4f);
                if (indexMusic >= int.Parse(gameObject.name) && 0 != int.Parse(gameObject.name))
                    Select();
                else if (0 != int.Parse(gameObject.name))
                    Unselect();
                if (indexMusic == 0 && 0 == int.Parse(gameObject.name))
                    Select();
                else if (0 == int.Parse(gameObject.name))
                    Unselect();
            }
            if (gameObject.tag == GV.TagSO._menuSon)
            {
                int indexMusic = (int)(PlayerPrefs.GetFloat("soundVolume", 0.5f) * 4f);
                if (indexMusic >= int.Parse(gameObject.name) && 0 != int.Parse(gameObject.name))
                    Select();
                else if (0 != int.Parse(gameObject.name))
                    Unselect();
                if (indexMusic == 0 && 0 == int.Parse(gameObject.name))
                    Select();
                else if (0 == int.Parse(gameObject.name))
                    Unselect();
            }

        }
    RaycastManager_.I.allTag[gameObject.tag]._click2DEvent.AddListener(() => SoundManager.I.F_PlaySound(GV.SoundSO._clicSurvole));
    }
    void FixedUpdate()
    {
        PlayCurve();
        VerifSelectedPourSurvole();
        VerifSurvoleback();
    }

    private IEnumerator ClickOnButtonCoroutine()
    {
        if(!gameObject.activeSelf)
            yield break;
        Select();
        yield return new WaitForSeconds(0.3f);
        Unselect();
    }

    private void VerifSelectedPourSurvole()
    {
        if (feedbackSurvole != null && isSelected && Mathf.Abs(carré1.transform.eulerAngles.z - 45f) > 0.1f)
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
        if (!isSelected && !survole && feedbackSurvole != null && !feedbackSurvole.IsPlaying && !feedbackSurvoleBack.IsPlaying && Mathf.Abs(carré1.transform.localEulerAngles.z - 45f) < 0.1f)
        {
            print(isSelected);
            feedbackSurvoleBack.PlayFeedbacks();
        }
        survole = false;
    }

    private void Survole()
    {
        survole = true;
        if (gameObject.activeSelf && feedbackSurvole != null && !feedbackSurvole.IsPlaying && !feedbackSurvoleBack.IsPlaying && MathF.Abs(carré1.transform.localEulerAngles.z) <= 0.1f && !isSelected)
        {
            feedbackSurvole.PlayFeedbacks();
            SoundManager.I.F_PlaySound(GV.SoundSO._boutonSurvole);
        }
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
            //VerifSurvoleback();
        }
    }

    private void OnEnable()
    {
        if (feedbackSurvole != null && !isSelected)
        {
            feedbackSurvole.StopFeedbacks();
            feedbackSurvoleBack.StopFeedbacks();
        }
    }

    //private void OnDisable()
    //{
    //    StopCoroutine(coroutine);
    //}
}
