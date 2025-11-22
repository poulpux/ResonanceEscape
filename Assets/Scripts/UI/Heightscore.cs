using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Heightscore : MonoBehaviour
{
    TextMeshPro text;
    [SerializeField] LanguageSupportSO langue;
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        SetRightHightscore();
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeLeftLevel]._click2DEvent.AddListener(() => SetRightHightscore());
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeRightLevel]._click2DEvent.AddListener(() => SetRightHightscore());
        GameManager.I._winTheLevelFeedbackEvent.AddListener(() => SetRightHightscore());
        GameManager.I._setRightLanguageEvent.AddListener(() => SetRightHightscore());
    }

    private void SetRightHightscore()
    {
        float time = PlayerPrefs.GetFloat(MenuManager.I._indexMapPlayMode.ToString(), 99.99f);
        text.text = langue.F_GetTextTranslation()+" : "+ $"{(int)time}.{Mathf.Floor((time % 1f) * 100f):00}";
    }
}
