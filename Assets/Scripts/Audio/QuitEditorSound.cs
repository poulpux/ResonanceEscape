using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitEditorSound : MonoBehaviour
{
    void Start()
    {
        RaycastManager_.I.allTag[GV.TagSO._editorBackToMenu]._click2DEvent.AddListener(() => SoundManager.I.F_PlaySound(GV.SoundSO._exitUI));
    }
}
