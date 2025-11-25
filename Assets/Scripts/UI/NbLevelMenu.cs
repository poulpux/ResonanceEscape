using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NbLevelMenu : MonoBehaviour
{
    TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshPro>();
        //RaycastManager_.I.allTag[GV.TagSO._menuPlayModeLeftLevel]._click2DEvent.AddListener(() => AssignText());
        //RaycastManager_.I.allTag[GV.TagSO._menuPlayModeRightLevel]._click2DEvent.AddListener(() => AssignText());
    }

    private void FixedUpdate()
    {
        AssignText();
    }

    private void AssignText()
    {
        text.text = "LV " + (MenuManager.I._indexMapPlayMode + 1).ToString();
    }

}
