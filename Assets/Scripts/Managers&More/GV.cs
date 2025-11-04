using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GV
{
    private static GameConfig _config;

    public static GameConfig Config
    {
        get
        {
            if (_config == null)
            {
                _config = AssetDatabase.LoadAssetAtPath<GameConfig>("Assets/SO/GameConfig/GameConfig.asset");
                if (_config == null)
                    Debug.LogError("⚠️ GameConfig.asset introuvable dans Resources !");
            }
            return _config;
        }
    }

    //// Accès directs raccourcis
    public static FeedbackSO FeedbackSO => Config.feedbackSO;
    public static PrefabSO PrefabSO => Config.prefabSO;
    public static ColorSO ColorSO => Config.colorSO;
    public static FontSO FontSO => Config.fontSO;
    public static SoundSO SoundSO => Config.soundSO;
    public static TagSO TagSO => Config.tagSO;
    public static GameSO GameSO => Config.gameSO;
}
