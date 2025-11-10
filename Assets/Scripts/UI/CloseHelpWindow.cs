using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseHelpWindow : MonoBehaviour
{
    void Start()
    {
        RaycastManager_.I.allTag[GV.TagSO._menuCloseHelpWindow]._click2DEvent.AddListener(()=> transform.parent.gameObject.SetActive(false));
    }
}
