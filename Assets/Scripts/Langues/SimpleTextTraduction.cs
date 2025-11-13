using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleTextTraduction : MonoBehaviour
{
    [SerializeField] LanguageSupportSO langue;
    [SerializeField] TextMeshPro text;
    void Start()
    {
        GameManager.I._setRightLanguageEvent.AddListener(() => SetAll());
    }

    private void SetAll()
    {
        text.text = langue.F_GetTextTranslation();
        SetFont();
    }

    private void SetFont()
    {
        if (GameManager.I._langueActuelle == ELangues.SIMPLIFIEDCHINESE
            || GameManager.I._langueActuelle == ELangues.TRADITIONNALCHINESE
            || GameManager.I._langueActuelle == ELangues.JAPANESE)
        {
            text.font = GV.FontSO._asianFont;
            text.fontStyle = GV.FontSO._asianFontBold ? FontStyles.Bold : FontStyles.Normal;
        }
        else if (GameManager.I._langueActuelle == ELangues.KOREAN)
        {
            text.font= GV.FontSO._koreanFont;
        }
        else
        {
            text.font = GV.FontSO._europeanFont;
            text.fontStyle = GV.FontSO._europeanFontBold ? FontStyles.Bold : FontStyles.Normal;
        }
        
    }
}
