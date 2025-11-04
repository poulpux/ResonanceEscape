using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Manager&More/Game Config")]
public class GameConfig : ScriptableObject
{
    // Ajoute ici tous tes futurs SO de configuration
    public PrefabSO prefabSO;
    public FeedbackSO feedbackSO;
    public ColorSO colorSO;
    public FontSO fontSO;
    public SoundSO soundSO;
    public TagSO tagSO;
    public GameSO gameSO;
}
