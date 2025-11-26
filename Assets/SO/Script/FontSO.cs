using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "FontSO", menuName = "SO/Font")]
public class FontSO : ScriptableObject
{
    public TMP_FontAsset _europeanFont, _asianFont, _europeanFontCredit;
    public bool _europeanFontBold, _asianFontBold;
}
