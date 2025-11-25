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
        MenuManager.I._changeLvEvent.AddListener(() => SetRightHightscore());
        GameManager.I._goToMenuEvent.AddListener(() => SetRightHightscore());
        GameManager.I._setRightLanguageEvent.AddListener(() => SetRightHightscore());
    }


    private void SetRightHightscore()
    {
        float time = MenuManager.I._heightScoreList[MenuManager.I._indexMapPlayMode];
        text.text = langue.F_GetTextTranslation()+" : "+ $"{(int)time}.{Mathf.Floor((time % 1f) * 100f):00}";
    }
}
