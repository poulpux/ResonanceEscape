using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Heightscore : MonoBehaviour
{
    TextMeshPro text;
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        SetRightHightscore();
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeLeftLevel]._click2DEvent.AddListener(() => SetRightHightscore());
        RaycastManager_.I.allTag[GV.TagSO._menuPlayModeRightLevel]._click2DEvent.AddListener(() => SetRightHightscore());
        GameManager.I._winTheLevelEvent.AddListener(() => SetRightHightscore());
    }

    private void SetRightHightscore()
    {
        float time = PlayerPrefs.GetFloat(MenuManager.I._indexMapPlayMode.ToString(), 99.99f);
        print(time);
        text.text = "HIGHTSCORE : "+ $"{(int)time}s{Mathf.Floor((time % 1f) * 100f):00}";
    }
}
