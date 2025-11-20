using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeelMapTypeButton : MonoBehaviour
{
    [SerializeField] MMF_Player map0, map1, map2;

    // Start is called before the first frame update
    void Start()
    {
        EditorManager.I._editorMapTypeFeedbackEvent.AddListener(() => SetRightFeedback());
        GameManager.I._enterInEditModeEvent.AddListener(()=> SetRightFeedback());
        GameManager.I._enterInEditModePastEvent.AddListener(()=> SetRightFeedback());
    }

    private void SetRightFeedback()
    {
        if (EditorManager.I.currentMapData._mapTypeC1 == 0)
            map0.PlayFeedbacks();
        else if (EditorManager.I.currentMapData._mapTypeC1 == 1)
            map1.PlayFeedbacks();
        else if (EditorManager.I.currentMapData._mapTypeC1 == 2)
            map2.PlayFeedbacks();
    }
}
