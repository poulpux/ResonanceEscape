using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageSupportSO", menuName = "SO/LanguageSupport")]
public class LanguageSupportSO : ScriptableObject
{
    [MultiLineProperty]
    public string
        _simplifiedChinese,
        _english,
        _russian,
        _spanish,
        _breazilian,
        _german,
        _french,
        _turkish,
        _japanese,
        _polish,
        _traditionnalChinese,
        _italian;

    public string F_GetTextTranslation()
    {
        if (GameManager.I._langueActuelle == ELangues.SIMPLIFIEDCHINESE) return _simplifiedChinese;
        else if (GameManager.I._langueActuelle == ELangues.TRADITIONNALCHINESE) return _traditionnalChinese;
        else if (GameManager.I._langueActuelle == ELangues.ENGLISH) return _english;
        else if (GameManager.I._langueActuelle == ELangues.RUSSIAN) return _russian;
        else if (GameManager.I._langueActuelle == ELangues.SPANISH) return _spanish;
        else if (GameManager.I._langueActuelle == ELangues.BREAZILIAN) return _breazilian;
        else if (GameManager.I._langueActuelle == ELangues.GERMAN) return _german;
        else if (GameManager.I._langueActuelle == ELangues.FRENCH) return _french;
        else if (GameManager.I._langueActuelle == ELangues.TURKISH) return _turkish;
        else if (GameManager.I._langueActuelle == ELangues.JAPANESE) return _japanese;
        else if (GameManager.I._langueActuelle == ELangues.POLISH) return _polish;
        else if (GameManager.I._langueActuelle == ELangues.ITALIAN) return _italian;

        return "NO LANGUAGE AVAILABLE";
    }
}
